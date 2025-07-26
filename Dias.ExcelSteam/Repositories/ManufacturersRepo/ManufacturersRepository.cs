using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.ManufacturersRepo
{
    internal class ManufacturersRepository : IManufacturersRepository
    {
        private readonly IDbConnection _connection;
        public ManufacturersRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<Manufacturer> GetAsync(ManufacturerDto manufacturer, IDbTransaction? transaction)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<Manufacturer>(
               "SELECT * FROM Manufacturers WHERE ManufacturerName = @Name",
              new { manufacturer.Name }, transaction);
            return result!;
        }

        
    }
}
