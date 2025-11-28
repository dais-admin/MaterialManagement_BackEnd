using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto>GetCategoryTypeById(Guid id);
        Task<CategoryDto>AddCategory(CategoryDto categoryDto);
        Task<CategoryDto>UpdateCategory(CategoryDto categoryDto);
        Task DeleteCategory(Guid id);
        Task<List<CategoryDto>> GetAllCategory();
        CategoryDto GetCategoryIdByName(string name);
        Task<IEnumerable<CategoryDto>> GetCategoriesByMaterialType(Guid typeId);
    }
}
