using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class FileInfoRepository : BaseRepository, IFileInfoRepository
    {
        public FileInfoRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<int> InsertAsync(FileInfoModel entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"INSERT INTO file_info (attribute, extension, parent_path, path, name, thumbnail_byte) VALUES 
                (@Attribute, @Extension, @ParentPath, @Path, @Name, @ThumbnailByte)",
                entity,
                _dbTransaction);
        }

        public async Task<int> InsertMultipleAsync(IEnumerable<FileInfoModel> entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"INSERT INTO file_info (attribute, extension, parent_path, path, name, thumbnail_byte) VALUES 
                (@Attribute, @Extension, @ParentPath, @Path, @Name, @ThumbnailByte) ON CONFLICT DO NOTHING",
                entity,
                _dbTransaction);
        }

        public async Task<IEnumerable<FileInfoModel>> GetAsync(string parentPath)
        {
            return await _dbConnection.QueryAsync<FileInfoModel>(
               @"SELECT * FROM file_info WHERE parent_path = @ParentPath",
               new { ParentPath = parentPath },
               _dbTransaction);
        }

        public async Task<IEnumerable<FileInfoModel>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<FileInfoModel>(
               @"SELECT * FROM file_info",
               null,
               _dbTransaction);
        }

        public async Task<int> GetRowCount(string parentPath)
        {
            return await _dbConnection.QueryFirstAsync<int>(
                @"SELECT COUNT(*) FROM file_info WHERE parent_path = @ParentPath",
                new { ParentPath = parentPath },
                _dbTransaction
                );
        }

        public async Task<int> UpdateAsync(FileInfoModel entity)
        {
            return await _dbConnection.ExecuteAsync(
                 @"UPDATE file_info SET thumbnail_byte = @ThumbnailByte WHERE id = @Id",
                 entity,
                 _dbTransaction);
        }

        public async Task<IEnumerable<FileInfoModel>> GetByPaginationAsync(string parentPath, int limit, int offset)
        {
            return await _dbConnection.QueryAsync<FileInfoModel>(
               @"SELECT * FROM file_INFO WHERE parent_path = @ParentPath LIMIT @Limit OFFSET @Offset",
               new { ParentPath = parentPath, Limit = limit, Offset = offset },
               _dbTransaction);
        }

        public async Task<int> DeleteAsync(IEnumerable<FileInfoModel> entity)
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM file_info WHERE id = @Id",
                entity,
                _dbTransaction);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM file_info",
                null,
                _dbTransaction);
        }
    }
}
