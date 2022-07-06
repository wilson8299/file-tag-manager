using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<int> InsertAsync(TagModel entity)
        {
            return await _dbConnection.QueryFirstAsync<int>(
               @"INSERT OR IGNORE INTO tag (name) VALUES (@Name) returning id",
               entity,
               _dbTransaction);
        }

        public async Task<int> UpsertAsync(TagModel entity)
        {
            return await _dbConnection.QueryFirstAsync<int>(
               @"INSERT INTO tag (name) VALUES (@Name) ON CONFLICT (name) DO UPDATE SET name = @Name returning id",
               entity,
               _dbTransaction);
        }

        public async Task<IEnumerable<TagModel>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<TagModel>(
               @"SELECT * FROM tag",
               null,
               _dbTransaction);
        }

        public async Task<int> GetByNameAsync(string tagName)
        {
            return await _dbConnection.QueryFirstAsync<int>(
               @"SELECT tag.id FROM tag WHERE tag.name == @TagName",
                new { TagName = tagName },
               _dbTransaction);
        }

        public async Task<IEnumerable<int>> GetByNamesAsync(IEnumerable<string> tagsName)
        {
            return await _dbConnection.QueryAsync<int>(
               @"SELECT tag.id FROM tag WHERE tag.name IN @TagsName",
                new { TagsName = tagsName },
               _dbTransaction);
        }
    }
}
