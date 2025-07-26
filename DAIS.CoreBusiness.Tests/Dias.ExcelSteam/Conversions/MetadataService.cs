using DAIS.DataAccess.Entities;
using Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dias.ExcelSteam.Conversions
{
    public class MetadataService(IDbConnection dbConnection) : IMetadataService
    {

        public async Task<IEnumerable<ExcelReaderMetadata>> GetExcelReaderMetadataAsync()
        {
            var result = await dbConnection.QueryAsync<ExcelReaderMetadata>(
                "SELECT * FROM ExcelReaderMetadata WHERE  IsRead = 0 AND IsDeleted=0");
            return result;
        }

        public async Task<int> UpdateExcelReaderMetadataAsync(Guid id, IDbTransaction? transaction)
        {
            var sql = "UPDATE ExcelReaderMetadata SET IsRead = 1 WHERE Id = @Id";
            var parameters = new { Id = id };
            return await dbConnection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<int> AddAsync(Material material, IDbTransaction? transaction)
        {
            var query = "INSERT INTO [dbo].[Materials] ([Id],[System],[TagNumber],[MaterialName]" +
                ",[MaterialCode],[MaterialQty],[LocationRFId],[PurchaseDate],[CommissioningDate]" +
                ",[YearOfInstallation],[DesignLifeDate],[EndPeriodLifeDate],[ModelNumber]" +
                ",[MaterialStatus],[TypeId],[CategoryId],[RegionId],[LocationId]" +
                ",[DivisionId],[ManufacturerId],[SupplierId],[WorkPackageId]," +
                "[IsRehabilitation],[CreatedBy]," +
                "[UpdatedBy],[CreatedDate],[UpdatedDate],[IsDeleted])" +
                " VALUES(@Id,@System,@TagNumber,@MaterialName,@MaterialCode,@MaterialQty," +
                "@LocationRFId,@PurchaseDate,@CommissioningDate,@YearOfInstallation," +
                "@DesignLifeDate,@EndPeriodLifeDate,@ModelNumber,@MaterialStatus,@TypeId,@CategoryId," +
                "@RegionId,@LocationId,@DivisionId,@ManufacturerId,@SupplierId,@WorkPackageId,@IsRehabilitation," +
                "@CreatedBy,@UpdatedBy,@CreatedDate,@UpdatedDate,@IsDeleted)";
            return await dbConnection.ExecuteAsync(query, material, transaction).ConfigureAwait(false);

        }
    }
}
