using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialDocumentService
    {
        Task<MaterialDocumentDto> AddMaterialDocumentAsync(MaterialDocumentDto materialDocumentDto);
        Task<MaterialDocumentDto> UpdateMaterialDocumentAsync(MaterialDocumentDto materialDocumentDto);
        Task<MaterialDocumentDto> DeleteMaterialDocumentAsync(Guid id);
        Task<MaterialDocumentDto> GetMaterialDocumentByIdAsync(Guid id);
        Task<List<MaterialDocumentDto>> GetAllMaterialDocumentAsync();
        Task<List<MaterialDocumentDto>> GetAllMaterialDocumentByMaterialIdAsync(Guid materialId);

    }
}
