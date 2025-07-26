using Dias.ExcelSteam.Connection;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dias.ExcelSteam.Repositories.UnitWork
{

    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public DapperUnitOfWork(IDbConnection sqlDbConnection)
        {
            _connection = sqlDbConnection;
            _connection.Open();
        }

        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public Task CommitAsync()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
            return Task.CompletedTask;
        }

        public IDbConnection Connection => _connection;

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }

    }
}
