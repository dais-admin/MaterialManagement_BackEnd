using Dias.ExcelSteam.Dtos;

namespace Dias.ExcelSteam.Repositories
{
    public interface IBulkInsertMaterial
    {
        Task InsertData(Guid messageId, MaterialDto materialDto, string? userName = null);
    }

    public class BulkInsertMaterial(AddMaterialService projectMaterialTypeService) : IBulkInsertMaterial
    {
        public async Task InsertData(Guid messageId, MaterialDto materialDto, string? userName = null)
        {
            await projectMaterialTypeService.CreateMaterialAsync(messageId, materialDto, userName);
        }
    }
}