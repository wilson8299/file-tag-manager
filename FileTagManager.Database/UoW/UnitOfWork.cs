using FileTagManager.Database.Repositories;
using FileTagManager.Domain.Interfaces;
using FileTagManager.Domain.Interfaces.Repositories;
using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace FileTagManager.Database.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string DatabasePath;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private IDirectoryInfoRepository _directoryInfoRepository;
        private IFileInfoRepository _fileInfoRepository;
        private IFileInfoFTSRepository _fileInfoFTSRepository;
        private ITagRepository _tagRepository;
        private ITagMapRepository _tagMapRepository;
        private IHistoryRepository _historyRepository;
        private ICombineRepository _combineRepository;
        private bool _disposed = false;

        public UnitOfWork()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            DatabasePath = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            _dbConnection = OpenConnection();
            _dbTransaction = _dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IDirectoryInfoRepository DirectoryInfoRepository
            => _directoryInfoRepository ??= new DirectoryInfoRepository(_dbTransaction);

        public IFileInfoRepository FileInfoRepository
            => _fileInfoRepository ??= new FileInfoRepository(_dbTransaction);

        public IFileInfoFTSRepository FileInfoFTSRepository
            => _fileInfoFTSRepository ??= new FileInfoFTSRepository(_dbTransaction);

        public ITagRepository TagRepository
            => _tagRepository ??= new TagRepository(_dbTransaction);

        public ITagMapRepository TagMapRepository
            => _tagMapRepository ??= new TagMapRepository(_dbTransaction);

        public IHistoryRepository HistoryRepository
            => _historyRepository ??= new HistoryRepository(_dbTransaction);

        public ICombineRepository CombineRepository
           => _combineRepository ??= new CombineRepository(_dbTransaction);

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch
            {
                _dbTransaction.Rollback();
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _directoryInfoRepository = null;
            _fileInfoRepository = null;
            _fileInfoFTSRepository = null;
            _tagRepository = null;
            _tagMapRepository = null;
            _historyRepository = null;
            _combineRepository = null;
        }

        private IDbConnection OpenConnection()
        {
            var connection = new SQLiteConnection(DatabasePath);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
                connection.EnableExtensions(true);
                connection.LoadExtension("SQLite.Interop.dll", "sqlite3_fts5_init");
            }
            return connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_dbTransaction != null)
                    {
                        _dbTransaction.Dispose();
                        _dbTransaction = null;
                    }
                    if (_dbConnection != null)
                    {
                        _dbConnection.Dispose();
                        _dbConnection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork() => Dispose(false);
    }
}
