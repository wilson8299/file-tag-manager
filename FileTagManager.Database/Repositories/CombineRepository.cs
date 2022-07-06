using Dapper;
using FileTagManager.Domain.Interfaces.Repositories;
using FileTagManager.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public class CombineRepository : BaseRepository, ICombineRepository
    {
        public CombineRepository(IDbTransaction dbTransaction) : base(dbTransaction) { }

        public async Task<IEnumerable<FileInfoFTSModel>> SearchAsync(
              string searchFileName,
              IEnumerable<string> searchTagName,
              int limit,
              int offset
              )
        {
            var sql = $@"SELECT ft.path, ft.name, ft.thumbnail_byte, rowid AS id FROM (SELECT *, rowid FROM file_info_fts WHERE name MATCH @SearchFileName ORDER BY RANK) ft
                                     UNION
                                     SELECT file_info_fts.*, file_info_fts.rowid FROM tag_map
                                     LEFT JOIN file_info_fts ON file_info_fts.rowid == tag_map.file_info_id
                                     WHERE tag_map.tag_id IN (SELECT tag.id FROM tag WHERE tag.name IN @SearchTagName)
                                     GROUP BY file_info_fts.rowid
                                     ORDER BY ft.rowid
                                     LIMIT @Limit OFFSET @Offset";

            return await _dbConnection.QueryAsync<FileInfoFTSModel>(
               sql,
               new { SearchFileName = searchFileName, SearchTagName = searchTagName, Limit = limit, Offset = offset },
               _dbTransaction);
        }

        public async Task<int> GetSearchRowCountAsync(
              string searchFileName,
              IEnumerable<string> searchTagName
              )
        {
            var sql = $@"SELECT COUNT(*) FROM (
	                                    SELECT ft.path, ft.name, ft.thumbnail_byte, rowid AS id FROM (SELECT *, rowid FROM file_info_fts WHERE name MATCH @SearchFileName ORDER BY RANK) ft
	                                    UNION
	                                    SELECT file_info_fts.*, file_info_fts.rowid FROM tag_map
	                                    LEFT JOIN file_info_fts ON file_info_fts.rowid == tag_map.file_info_id
	                                    WHERE tag_map.tag_id IN (SELECT tag.id FROM tag WHERE tag.name IN @SearchTagName)
                                    )";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(
               sql,
               new { SearchFileName = searchFileName, SearchTagName = searchTagName },
               _dbTransaction);
        }

        public async Task<IEnumerable<ExportModel>> ExportAsync()
        {
            return await _dbConnection.QueryAsync<ExportModel>(
                $@"SELECT *, group_concat(tag_name) as tag FROM 
                        (
	                        SELECT file.*, tag.name as tag_name FROM (SELECT rowid as id, path, name, thumbnail_byte FROM file_info_fts WHERE thumbnail_byte IS NOT NULL) file
		                        LEFT JOIN tag_map ON file.id == tag_map.file_info_id
		                        LEFT JOIN tag ON tag_map.tag_id == tag.id
	                        UNION
	                        SELECT file_info_fts.rowid as id, file_info_fts.*, tag.name as tag_name FROM tag_map
		                        LEFT JOIN tag ON tag_map.tag_id == tag.id
		                        LEFT JOIN file_info_fts ON file_info_fts.rowid == tag_map.file_info_id
                        )
                        GROUP BY id;",
                null,
                _dbTransaction);
        }

        public async Task<IEnumerable<TagModel>> GetFileTags(int fileInfoId)
        {
            return await _dbConnection.QueryAsync<TagModel>(
                $@"SELECT tag_map.tag_id as id, tag.name FROM tag_map
                        LEFT JOIN tag ON tag.id = tag_map.tag_id
                        WHERE tag_map.file_info_id =@FileInfoId",
                new { FileInfoId = fileInfoId },
                _dbTransaction);
        }
    }
}
