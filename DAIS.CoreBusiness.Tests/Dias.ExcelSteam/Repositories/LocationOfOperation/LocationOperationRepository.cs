using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.Repositories.LocationOfOperation
{
    public class LocationOperationRepository : ILocationOperationRepository
    {
        private readonly IDbConnection _connection;
        public LocationOperationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<LocationOperation> GetAsync(LocationOfOperationDto locationOf, IDbTransaction? transaction)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<LocationOperation>(
             "SELECT * FROM LocationOperations WHERE LocationOperationName = @Name",
            new { locationOf.Name }, transaction);
            return result!;
        }

        public async Task<LocationOperation> InsertAsync(LocationOfOperationDto locationOf, Guid? projectId, IDbTransaction? transaction)
        {
            var result = await GetAsync(locationOf, transaction);
            if (result != null)
            {
                return result;
            }
            var cate = new LocationOperation
            {
                LocationOperationName = locationOf.Name
               
            };
            var query = "INSERT INTO LocationOperations (Id, LocationOperationCode, LocationOperationName,CreatedDate,UpdatedDate,IsDeleted) VALUES (@Id, @LocationOperationName,  @CreatedDate,@UpdatedDate,@IsDeleted)";
            await _connection.ExecuteAsync(query, cate, transaction);
            return cate;
        }
    }
}
