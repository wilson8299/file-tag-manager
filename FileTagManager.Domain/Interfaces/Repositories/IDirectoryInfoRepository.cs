using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface IDirectoryInfoRepository
    {
        Task<int> InsertAsync(DirectoryInfoModel entity);
        Task<int> InsertMultipleAsync(IEnumerable<DirectoryInfoModel> entity);
        Task<int> GetByPathAsync(string path);
        Task<IEnumerable<DirectoryInfoModel>> GetAllAsync();
        Task<int> DeleteAsync(IEnumerable<DirectoryInfoModel> entity);
        Task<int> DeleteAllAsync();
    }
}
