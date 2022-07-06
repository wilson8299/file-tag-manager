using FileTagManager.Domain.Models;
using FileTagManager.WPF.Helpers;
using FileTagManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileTagManager.WPF.Services
{
    public class FileService : IFileService
    {
        private readonly IFileSystem _fileSystem;

        public FileService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public DirectoryInfo SigleDirectoryInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        public IEnumerable<(FileInfoModel, bool)> GetInfo(string path)
        {
            var directory = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            IEnumerable<IFileSystemInfo> files = directory.EnumerateFileSystemInfos("*", enumerationOptions: new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            });

            foreach (var file in files)
            {
                var isDirectory = file.Attributes == FileAttributes.Directory;
                var fileInfo = new FileInfoModel
                {
                    Attribute = file.Attributes.ToString(),
                    Extension = file.Extension,
                    ParentPath = _fileSystem.Directory.GetParent(file.FullName).FullName,
                    Path = file.FullName,
                    Name = file.Name,
                };

                yield return (fileInfo, isDirectory);
            }
        }

        public ObservableCollection<ExplorerVeiwModel> GetDirectoriesTree(IEnumerable<DirectoryInfoModel> directoryInfo, int parentId)
        {
            var directoryCollection = new ObservableCollection<ExplorerVeiwModel>();
            foreach (var item in directoryInfo.Where(d => d.ParentId == parentId))
            {
                var directory = new ExplorerVeiwModel()
                {
                    Name = item.Name,
                    Path = item.Path,
                    Children = GetDirectoriesTree(directoryInfo, item.Id)
                };
                directoryCollection.Add(directory);
            }
            return directoryCollection;
        }

        public IEnumerable<DirectoryInfoModel> GetDirectories(string path)
        {
            var directory = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            if (!directory.Exists) return null;
            var directories = directory.EnumerateDirectories("*", enumerationOptions: new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            });

            var directoryInfoList = new List<DirectoryInfoModel>();
            foreach (var dir in directories)
            {
                directoryInfoList.Add(new DirectoryInfoModel
                {
                    ParentPath = _fileSystem.Directory.GetParent(dir.FullName).FullName,
                    Path = dir.FullName,
                    Name = dir.Name
                });
            }
            return directoryInfoList;
        }

        public IEnumerable<FileInfoModel> GetFiles(string path)
        {
            var directory = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            if(!directory.Exists) return null;
            IEnumerable<IFileSystemInfo> files = directory.EnumerateFileSystemInfos("*", enumerationOptions: new EnumerationOptions
            {
                RecurseSubdirectories = false,
                IgnoreInaccessible = true
            });

            var fileInfoList = new List<FileInfoModel>();
            foreach (var file in files)
            {
                fileInfoList.Add(new FileInfoModel
                {
                    Attribute = file.Attributes.ToString(),
                    Extension = file.Extension,
                    ParentPath = _fileSystem.Directory.GetParent(file.FullName).FullName,
                    Path = file.FullName,
                    Name = file.Name,
                });
            }
            return fileInfoList;
        }

        public ObservableCollection<FileInfoModel> GetFileThumbnail(IEnumerable<FileInfoModel> fileInfo)
        {
            foreach (var file in fileInfo)
            {
                file.Thumbnail = file.ThumbnailByte == null || file.ThumbnailByte.Length <= 0
                    ? GetIThumbnailByPath(file.Path)
                    : GetIThumbnailByByte(file.ThumbnailByte);
            }
            return new ObservableCollection<FileInfoModel>(fileInfo);
        }

        public ImageSource GetIThumbnailByPath(string path)
        {
            bool isFileExists = File.Exists(path);
            bool isDirExists = Directory.Exists(path);
            if (isFileExists || isDirExists)
            {
                return ThumbnailHelper.GetThumbnail(path, 256, 256, ThumbnailOptions.NONE).Bitmap2ImageSource();
            }
            else
            {
                return ImageHelper.Uri2ImageSource(new Uri("pack://application:,,,/Assets/x.png"));
            }
        }

        public ImageSource GetIThumbnailByByte(byte[] imgByte)
        {
            return imgByte.Byte2ImageSource();
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public byte[] ReadFileByte(string path)
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var imgByte = new byte[fs.Length];
            fs.Read(imgByte, 0, imgByte.Length);
            return imgByte;
        }

        public void WriteFile(string path, string data)
        {
            File.WriteAllText(path, data);
        }

        public void OpenFile(string path)
        {
            new Process { 
                StartInfo = new ProcessStartInfo(path) { UseShellExecute = true } 
            }.Start();

            //ShellHelper.ShellExecute(IntPtr.Zero, null, path, null, null, ShellHelper.ShowCommands.SW_NORMAL);
        }

        #region Fake async
        public async Task<ObservableCollection<FileInfoModel>> GetFileThumbnailAsync(IEnumerable<FileInfoModel> fileInfo)
        {
            return await Task.Run(() => GetFileThumbnail(fileInfo));
        }

        public async Task<ObservableCollection<ExplorerVeiwModel>> GetDirectoriesAsync(IEnumerable<DirectoryInfoModel> directoryInfo, int parentId)
        {
            return await Task.Run(() => GetDirectoriesTree(directoryInfo, parentId));
        }

        public async Task<IEnumerable<DirectoryInfoModel>> GetDirectoriesAsync(string path)
        {
            return await Task.Run(() => GetDirectories(path));
        }

        public async Task<IEnumerable<FileInfoModel>> GetFilesAsync(string path)
        {
            return await Task.Run(() => GetFiles(path));
        }
        #endregion
    }
}
