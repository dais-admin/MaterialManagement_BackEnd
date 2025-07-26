using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.RegionRepo
{
    public class RegionRepository(IDbConnection connection) : IRegionRepository
    {
        public async Task<Region> GetAsync(RegionDto region, IDbTransaction? transaction)
        {
            var result = await connection.QueryFirstOrDefaultAsync<Region>("SELECT * FROM Regions WHERE RegionName = @Name", new { region.Name }, transaction);
            return result!;
        }

        public async Task<Region> InsertAsync(RegionDto Region, Guid? projectId, IDbTransaction? transaction)
        {
            var result = await GetAsync(Region, transaction);
            if (result != null)
            {
                return result;
            }
            var cate = new Region
            {
                RegionName = Region.Name
            };
            var query = "INSERT INTO Regions (Id, RegionCode, RegionName,CreatedDate,UpdatedDate,IsDeleted) VALUES (@Id, @RegionName, @RegionCode, @CreatedDate,@UpdatedDate,@IsDeleted)";
            await connection.ExecuteAsync(query, cate, transaction);
            return cate;
        }
    }
}
