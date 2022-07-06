using FileTagManager.Domain.Models;
using System.Threading.Tasks;

namespace FileTagManager.Domain.Interfaces.Repositories
{
    public interface IFileInfoFTSRepository
    {
        Task<FileInfoFTSModel> GetByName(string name);
    }
}
