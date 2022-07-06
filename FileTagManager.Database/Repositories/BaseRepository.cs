using FileTagManager.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileTagManager.Database.Repositories
{
    public abstract class BaseRepository
    {
        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection => _dbTransaction.Connection;

        public BaseRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }
    }
}
