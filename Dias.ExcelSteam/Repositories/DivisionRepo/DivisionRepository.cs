using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.DivisionRepo
{
    public class DivisionRepository(IDbConnection connection) : IDivisionRepository
    {
        public async Task<Division> GetAsync(DivisionDto divisionDto, IDbTransaction? transaction)
        {
            var result = await connection.QueryFirstOrDefaultAsync<Division>(
                "SELECT * FROM Divisions WHERE DivisionName = @Name", new { divisionDto.Name }, transaction);
            return result!;
        }
    }
}
