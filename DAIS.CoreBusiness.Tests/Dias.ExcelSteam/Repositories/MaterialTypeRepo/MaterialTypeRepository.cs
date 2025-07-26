using DAIS.DataAccess.Entities;
using Dapper;
using Dias.ExcelSteam.Dtos;
using System.Data;

namespace Dias.ExcelSteam.Repositories.MaterialTypeRepo
{
    public class MaterialTypeRepository(IDbConnection connection) : IMaterialTypeRepository
    {
        public async Task<MaterialType> GetAsync(MaterialTypeDto materialType, IDbTransaction transaction)
        {
            var result = await connection.QueryFirstOrDefaultAsync<MaterialType>(
                "SELECT * FROM MaterialTypes WHERE TypeName = @Name",
                new { materialType.Name },
                transaction);
            return result!;
        }

        public async Task<MaterialType> InsertAsync(MaterialTypeDto materialTypeDto, Guid projectId, IDbTransaction transaction)
        {
            var result = await GetAsync(materialTypeDto, transaction).ConfigureAwait(false);
            if (result is not null)
            {
                return result;
            }

            var materialType = new MaterialType
            {
                TypeName = materialTypeDto.Name,
               ProjectId = projectId
            };

            var insertMaterialTypeQuery = "INSERT INTO MaterialTypes (Id, TypeName, TypeCode, ProjectId,CreatedDate,UpdatedDate,IsDeleted) VALUES (@Id, @TypeName, @TypeCode, @ProjectId, @CreatedDate,@UpdatedDate,@IsDeleted)";
            await connection.ExecuteAsync(insertMaterialTypeQuery, materialType, transaction);
            return materialType;
        }
    }
}
