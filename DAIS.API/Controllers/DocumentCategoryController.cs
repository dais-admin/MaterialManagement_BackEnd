using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentCategoryController : ControllerBase
    {
        private readonly IDocumentCategoryService _documentCategoryService;

        public DocumentCategoryController(IDocumentCategoryService documentCategoryService)
        {
            _documentCategoryService = documentCategoryService;
        }

        [HttpGet("GetAllDocumentCategoryMasterList")]
        public async Task<IActionResult> GetAllDocumentCategoryMasterList()
        {
            return Ok(await _documentCategoryService.GetAllDocumentCategoryAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddDocumentCategoryMaster(DocumentCategoryDto documentCategoryDto)
        {
            return Ok(await _documentCategoryService.AddDocumentCategoryAsync(documentCategoryDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocumentCategoryAsync(DocumentCategoryDto documentCategoryDto)
        {
            return Ok(await _documentCategoryService.UpdateDocumentCategoryAsync(documentCategoryDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDocumentCategoryAsync(Guid id)
        {
            await _documentCategoryService.DeleteDocumentCategoryAsync(id);
            return Ok();
        }

        [HttpGet("GetDocumentCategoryById")]
        public async Task<IActionResult> GetDocumentCategoryById(Guid id)
        {
            var documentCategory = await _documentCategoryService.GetDocumentCategoryByIdAsync(id);
            return Ok(documentCategory);
        }
    }
}
