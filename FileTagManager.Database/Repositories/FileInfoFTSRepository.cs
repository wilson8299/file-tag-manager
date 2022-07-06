using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class FileInfoFTSRepository : BaseRepository, IFileInfoFTSRepository
    {
        public FileInfoFTSRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<FileInfoFTSModel> GetByName(string name)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<FileInfoFTSModel>(
                @"SELECT *, rowid as id FROM file_info_fts WHERE name MATCH @Name",
                new { Name = name },
                _dbTransaction);
        }
    }
}
