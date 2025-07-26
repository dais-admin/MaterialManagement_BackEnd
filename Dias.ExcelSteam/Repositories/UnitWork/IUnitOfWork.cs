using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.UnitWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction BeginTransaction();
        Task CommitAsync();
        IDbConnection Connection { get; }
    }
}
