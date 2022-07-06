using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface ICombineRepository
    {
        Task<IEnumerable<FileInfoFTSModel>> SearchAsync(string searchFileName, IEnumerable<string> searchTagName, int limit, int offset);
        Task<int> GetSearchRowCountAsync(string searchFileName, IEnumerable<string> searchTagName);
        Task<IEnumerable<ExportModel>> ExportAsync();
        Task<IEnumerable<TagModel>> GetFileTags(int fileInfoId);
    }
}
