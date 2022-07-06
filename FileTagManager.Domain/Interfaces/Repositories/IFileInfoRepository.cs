using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface IFileInfoRepository
    {
        Task<int> InsertAsync(FileInfoModel entity);
        Task<int> InsertMultipleAsync(IEnumerable<FileInfoModel> entity);
        Task<IEnumerable<FileInfoModel>> GetAllAsync();
        Task<int> UpdateAsync(FileInfoModel entity);
        Task<IEnumerable<FileInfoModel>> GetAsync(string parentPath);
        Task<int> GetRowCount(string parentPath);
        Task<IEnumerable<FileInfoModel>> GetByPaginationAsync(string parentPath, int limit, int offset);
        Task<int> DeleteAsync(IEnumerable<FileInfoModel> entity);
        Task<int> DeleteAllAsync();
    }
}
