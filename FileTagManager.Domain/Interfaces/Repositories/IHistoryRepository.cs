using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface IHistoryRepository
    {
        Task<int> InsertAsync(HistoryModel entity);
        Task<IEnumerable<HistoryModel>> GetAllAsync();
        Task<int> DeleteAllAsync();
    }
}
