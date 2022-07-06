using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace FileTagManager.WPF.Helpers
{
    [Flags]
    public enum ThumbnailOptions
    {
        NONE = 0x00,
        BIGGER_SIZE_OK = 0x01,
        IN_MEMORY_ONLY = 0x02,
        IconOnly = 0x04,
        ThumbnailOnly = 0x08,
        InCacheOnly = 0x10,
    }

    public static class ThumbnailHelper
    {
        private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            IntPtr pbc,
            ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        internal interface IShellItem
        {
            void BindToHandler(IntPtr pbc,
                [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
                out IntPtr ppv);

            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(int sfgaoMask, out int psfgaoAttribs);
            void Compare(IShellItem psi, int hint, out int piOrder);
        };

        internal enum SIGDN : uint
        {
            NORMAL_DISPLAY = 0,
            PARENT_RELATIVE_PARSING = 0x80018001,
            PARENT_RELATIVE_FOR_ADDRESSBAR = 0x8001c001,
            DESKTOP_ABSOLUTE_PARSING = 0x80028000,
            PARENT_RELATIVE_EDITING = 0x80031001,
            DESKTOP_ABSOLUTE_EDITING = 0x8004c000,
            FILE_SYS_PATH = 0x80058000,
            URL = 0x80068000
        }

        internal enum HResult
        {
            OK = 0x0000,
            FALSE = 0x0001,
            INVALID_ARGUMENTS = unchecked((int)0x80070057),
            OUT_OF_MEMORY = unchecked((int)0x8007000E),
            NO_NTERFACE = unchecked((int)0x80004002),
            FAIL = unchecked((int)0x80004005),
            ELEMENT_NOT_FOUND = unchecked((int)0x80070490),
            TYPEELEMENT_NOT_FOUND = unchecked((int)0x8002802B),
            NO_OBJECT = unchecked((int)0x800401E5),
            WIN32_ERROR_CANCELED = 1223,
            CANCELED = unchecked((int)0x800704C7),
            RESOURCE_INUSE = unchecked((int)0x800700AA),
            ACCESS_DENIED = unchecked((int)0x80030005)
        }

        [ComImportAttribute()]
        [GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IShellItemImageFactory
        {
            [PreserveSig]
            HResult GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] NativeSize size,
            [In] ThumbnailOptions flags,
            [Out] out IntPtr phbm);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeSize
        {
            private int width;
            private int height;

            public int Width { set { width = value; } }
            public int Height { set { height = value; } }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public static Bitmap GetThumbnail(string fileName, int width, int height, ThumbnailOptions options)
        {
            IntPtr hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);

            try
            {
                return GetBitmapFromHBitmap(hBitmap);
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        public static Bitmap GetBitmapFromHBitmap(IntPtr nativeHBitmap)
        {
            Bitmap bmp = Bitmap.FromHbitmap(nativeHBitmap);
            if (Bitmap.GetPixelFormatSize(bmp.PixelFormat) < 32)  return bmp;
            return CreateAlphaBitmap(bmp, PixelFormat.Format32bppArgb);
        }

        public static Bitmap CreateAlphaBitmap(Bitmap srcBitmap, PixelFormat targetPixelFormat)
        {
            Bitmap result = new Bitmap(srcBitmap.Width, srcBitmap.Height, targetPixelFormat);
            Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
            BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
            bool isAlplaBitmap = false;

            try
            {
                for (int y = 0; y <= srcData.Height - 1; y++)
                {
                    for (int x = 0; x <= srcData.Width - 1; x++)
                    {
                        Color pixelColor = Color.FromArgb(
                            Marshal.ReadInt32(srcData.Scan0, (srcData.Stride * y) + (4 * x)));

                        if (pixelColor.A > 0 & pixelColor.A < 255)
                        {
                            isAlplaBitmap = true;
                        }
                        result.SetPixel(x, y, pixelColor);
                    }
                }
            }
            finally
            {
                srcBitmap.UnlockBits(srcData);
            }

            return isAlplaBitmap ? result : srcBitmap;
        }

        private static IntPtr GetHBitmap(string fileName, int width, int height, ThumbnailOptions options)
        {
            IShellItem nativeShellItem;
            Guid shellItem2Guid = new Guid(IShellItem2Guid);
            int retCode = SHCreateItemFromParsingName(fileName, IntPtr.Zero, ref shellItem2Guid, out nativeShellItem);

            if (retCode != 0) throw Marshal.GetExceptionForHR(retCode);

            NativeSize nativeSize = new NativeSize();
            nativeSize.Width = width;
            nativeSize.Height = height;

            IntPtr hBitmap;
            HResult hr;
            hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, options, out hBitmap);

            if (hr == HResult.OK)
            {
                Marshal.ReleaseComObject(nativeShellItem);
                return hBitmap;
            }
            else
            {
                hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, ThumbnailOptions.NONE, out hBitmap);
                Marshal.ReleaseComObject(nativeShellItem);
                return hBitmap;
            }

            throw Marshal.GetExceptionForHR((int)hr);
        }
    }
}
