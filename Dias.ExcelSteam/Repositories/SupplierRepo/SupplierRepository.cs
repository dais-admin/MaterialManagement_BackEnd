using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using System.Data;
using Dapper;

namespace Dias.ExcelSteam.Repositories.SupplierRepo
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDbConnection _connection;
        public SupplierRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Supplier> GetAsync(SupplierDto supplier, IDbTransaction? transaction)
        {
            var package = await _connection.QueryFirstOrDefaultAsync<Supplier>(
           "SELECT * FROM Suppliers WHERE SupplierName = @Name",
           new { supplier.Name },
           transaction);
            return package!;
        }

    }
}
