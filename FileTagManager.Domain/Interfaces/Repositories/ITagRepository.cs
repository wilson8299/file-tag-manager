using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<int> InsertAsync(TagModel entity);
        Task<int> UpsertAsync(TagModel entity);
        Task<IEnumerable<TagModel>> GetAllAsync();
        Task<int> GetByNameAsync(string tagName);
        Task<IEnumerable<int>> GetByNamesAsync(IEnumerable<string> tagsName);
    }
}
