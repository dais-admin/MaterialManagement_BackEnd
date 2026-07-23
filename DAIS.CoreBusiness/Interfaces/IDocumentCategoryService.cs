using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDocumentCategoryService
    {
        Task<DocumentCategoryDto> AddDocumentCategoryAsync(DocumentCategoryDto documentCategoryDto);
        Task<DocumentCategoryDto> UpdateDocumentCategoryAsync(DocumentCategoryDto documentCategoryDto);
        Task DeleteDocumentCategoryAsync(Guid id);
        Task<DocumentCategoryDto> GetDocumentCategoryByIdAsync(Guid id);
        Task<List<DocumentCategoryDto>> GetAllDocumentCategoryAsync();
    }
}
