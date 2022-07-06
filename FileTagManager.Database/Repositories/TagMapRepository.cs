using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class TagMapRepository : BaseRepository, ITagMapRepository
    {
        public TagMapRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<int> InsertAsync(TagMapModel entity)
        {
            return await _dbConnection.ExecuteAsync(
               @"INSERT OR IGNORE INTO tag_map (file_info_id, tag_id) VALUES (@FileInfoId, @TagId)",
               entity,
               _dbTransaction);
        }

        public async Task<IEnumerable<TagMapModel>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<TagMapModel>(
               @"SELECT * FROM tag_map",
               null,
               _dbTransaction);
        }

        public async Task<int> DeleteAsync(TagMapModel entity)
        {
            return await _dbConnection.ExecuteAsync(
               @"DELETE FROM tag_map WHERE tag_id = @TagId AND file_info_id = @FileInfoId",
                entity,
               _dbTransaction);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM tag_map",
                null,
                _dbTransaction);
        }
    }
}
