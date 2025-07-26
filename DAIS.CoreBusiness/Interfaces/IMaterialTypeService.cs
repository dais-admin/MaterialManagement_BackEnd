using DAIS.CoreBusiness.Dtos;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialTypeService
    {
        Task<MaterialTypeDto> GetMaterialTypeById(Guid id);
        Task<MaterialTypeDto> AddMaterialType(MaterialTypeDto materialTypeDto);
        Task<MaterialTypeDto> UpdateMaterialType(MaterialTypeDto materialTypeDto);
        Task DeleteMaterialType(Guid id);
        Task<List<MaterialTypeDto>> GetAllMaterialTypes();
        MaterialTypeDto GetMaterialTypeIdByName(string name);
    }
}
