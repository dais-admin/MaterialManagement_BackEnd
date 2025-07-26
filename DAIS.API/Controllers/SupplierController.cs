using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly MaterialConfigSettings _materialConfig;
        public SupplierController(ISupplierService supplierService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _supplierService = supplierService;
            _materialConfig = materialConfig.Value;
        }
        [HttpGet("GetAllSupplier")]
        public async Task<IActionResult> GetAllSupplier()
        {
            var listSupplier = await _supplierService.GetAllSupplier();
            return Ok(listSupplier);
        }
        [HttpGet("GetSupplier")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var listSupplier = await _supplierService.GetSupplier(id);
            return Ok(listSupplier);
        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromForm] IFormFile? supplierDocument, [FromForm] string supplierData)
        {
            if (supplierData == null || string.IsNullOrEmpty(supplierData))
            {
                return BadRequest();
            }
            var supplierDto = JsonConvert.DeserializeObject<SupplierDto>(supplierData);

            if (supplierDocument != null)
            {
                supplierDto.SupplierDocument = await SaveSupplierDocument(supplierDocument);
            }
            var response = await _supplierService.AddSupplier(supplierDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSupplier([FromForm] IFormFile? supplierDocument, [FromForm] string supplierData)
        {
            if (supplierData == null || string.IsNullOrEmpty(supplierData))
            {
                return BadRequest();
            }
            var supplierDto = JsonConvert.DeserializeObject<SupplierDto>(supplierData);

            if (supplierDocument != null)
            {
                supplierDto.SupplierDocument = await SaveSupplierDocument(supplierDocument);
            }
            var response = await _supplierService.UpdateSupplier(supplierDto);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            await _supplierService.DeleteSupplier(id);
            return Ok();
        }
        private async Task<string> SaveSupplierDocument([FromForm] IFormFile? documentFile)
        {
            var folderName = "MaterialDocument" + "\\" + "SupplierDocuments" + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = documentFile.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await documentFile.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }
    }
}
