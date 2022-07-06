using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class HistoryRepository : BaseRepository, IHistoryRepository
    {
        public HistoryRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<int> InsertAsync(HistoryModel entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"INSERT INTO history (name) VALUES (@Name)",
                entity,
                _dbTransaction);
        }

        public async Task<IEnumerable<HistoryModel>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<HistoryModel>(
                @"SELECT * FROM history ORDER BY id DESC",
                null,
                _dbTransaction);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM history",
                null,
                _dbTransaction);
        }
    }
}
