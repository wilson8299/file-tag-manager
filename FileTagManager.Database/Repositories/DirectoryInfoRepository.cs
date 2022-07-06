using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class DirectoryInfoRepository : BaseRepository, IDirectoryInfoRepository
    {
        public DirectoryInfoRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<int> InsertAsync(DirectoryInfoModel entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"INSERT INTO directory_info (path, name, parent_id, parent_path) VALUES (@Path, @Name, @ParentId, @ParentPath)",
                entity,
                _dbTransaction);
        }

        public async Task<int> InsertMultipleAsync(IEnumerable<DirectoryInfoModel> entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"INSERT INTO directory_info (path, name, parent_id, parent_path) VALUES (@Path, @Name, @ParentId, @ParentPath) ON CONFLICT DO NOTHING",
                entity,
                _dbTransaction);
        }

        public async Task<int> GetByPathAsync(string path)
        {
            return await _dbConnection.QueryFirstAsync<int>(
                @"SELECT id FROM directory_info WHERE path = @Path",
                new { Path = path },
                _dbTransaction);
        }

        public async Task<IEnumerable<DirectoryInfoModel>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<DirectoryInfoModel>(
                @"SELECT * FROM directory_info",
                null,
                _dbTransaction);
        }

        public async Task<int> DeleteAsync(IEnumerable<DirectoryInfoModel> entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM directory_info WHERE id = @Id",
                entity,
                _dbTransaction);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM directory_info",
                null,
                _dbTransaction);
        }
    }
}
