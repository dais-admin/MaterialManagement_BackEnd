using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IInventoryService
    {
        Task<MaterialInventoryDto> GetMaterialInventoryByCodeAsync(string materialCode);
        Task<MaterialDto> UpdateMaterialAsync(MaterialDto materialDto);
    }
}
