using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController( ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        
        }
        [HttpGet("GetAllCategory")]
        public  async Task<IActionResult>GetAllCategory()
        {
            var listCategory= await _categoryService.GetAllCategory();
            return Ok(listCategory);
        }
        [HttpGet("GetCategory")]
        public async Task<IActionResult>GetCategory(Guid id)
        {
            var listCategory=await _categoryService.GetCategoryTypeById(id);
            return Ok(listCategory);
        }
        [HttpGet("GetCategoriesByMaterialType")]
        public async Task<IActionResult> GetCategoriesByMaterialType(Guid typeId)
        {
            var categories = await _categoryService.GetCategoriesByMaterialType(typeId);
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            return Ok(await _categoryService.AddCategory(categoryDto));
        }

        [HttpPut]
        public async Task<IActionResult>UpdateCategory(CategoryDto categoryDto)
        {
            return Ok(await _categoryService.UpdateCategory(categoryDto));
        }

        [HttpDelete]
        public async Task<IActionResult>DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
