using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentMasterController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;
        public DocumentMasterController(IDocumentTypeService documentTypeService) 
        {
            _documentTypeService = documentTypeService;
        }

        [HttpGet("GetAllDocumnetMasterList")]
        public async Task<IActionResult> GetAllDocumnetMasterList()
        {
            return Ok(await _documentTypeService.GetAllDocumentAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddDocumentMaster(DocumentMasterDto documentMasterDto)
        {
            return Ok(await _documentTypeService.AddDocumentAsync(documentMasterDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDocumentAsync(DocumentMasterDto documentMasterDto)
        {
            return Ok(await _documentTypeService.UpdateDocumentAsync(documentMasterDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDocumentAsync(Guid id)
        {
            await _documentTypeService.DeleteDocumentAsync(id);
            return Ok();
        }
        [HttpGet("GetDocumentById")]
        public  async Task<IActionResult> GetDocumentById(Guid id)
        {
            var document = await _documentTypeService.GetDocumentByIdAsync(id);
            return Ok(document);

        }
    }
}
