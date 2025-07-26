using DAIS.CoreBusiness.Dtos;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<DocumentMasterDto> AddDocumentAsync(DocumentMasterDto documentMasterDto);
        Task<DocumentMasterDto> UpdateDocumentAsync(DocumentMasterDto documentMasterDto);
        Task DeleteDocumentAsync(Guid id);
        Task<DocumentMasterDto> GetDocumentByIdAsync(Guid id);
        Task<List<DocumentMasterDto>> GetAllDocumentAsync();
        
    }
}
