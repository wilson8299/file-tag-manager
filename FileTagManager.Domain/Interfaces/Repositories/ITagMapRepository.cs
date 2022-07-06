using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface ITagMapRepository
    {
        Task<int> InsertAsync(TagMapModel entity);
        Task<IEnumerable<TagMapModel>> GetAllAsync();
        Task<int> DeleteAsync(TagMapModel entity);
        Task<int> DeleteAllAsync();
    }
}
