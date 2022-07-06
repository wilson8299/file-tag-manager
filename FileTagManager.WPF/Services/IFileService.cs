using FileTagManager.Domain.Models;
using FileTagManager.WPF.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileTagManager.WPF.Services
{
    public interface IFileService
    {
        DirectoryInfo SigleDirectoryInfo(string path);
        IEnumerable<(FileInfoModel, bool)> GetInfo(string path);
        ObservableCollection<ExplorerVeiwModel> GetDirectoriesTree(IEnumerable<DirectoryInfoModel> directoryInfo, int parentId);
        ObservableCollection<FileInfoModel> GetFileThumbnail(IEnumerable<FileInfoModel> fileInfo);
        Task<ObservableCollection<FileInfoModel>> GetFileThumbnailAsync(IEnumerable<FileInfoModel> fileInfo);
        Task<ObservableCollection<ExplorerVeiwModel>> GetDirectoriesAsync(IEnumerable<DirectoryInfoModel> directoryInfo, int parentId);
        ImageSource GetIThumbnailByPath(string path);
        ImageSource GetIThumbnailByByte(byte[] imgByte);
        string ReadFile(string path);
        byte[] ReadFileByte(string path);
        void WriteFile(string path, string data);
        void OpenFile(string path);
        IEnumerable<DirectoryInfoModel> GetDirectories(string path);
        IEnumerable<FileInfoModel> GetFiles(string path);
        Task<IEnumerable<DirectoryInfoModel>> GetDirectoriesAsync(string path);
        Task<IEnumerable<FileInfoModel>> GetFilesAsync(string path);
    }
}
