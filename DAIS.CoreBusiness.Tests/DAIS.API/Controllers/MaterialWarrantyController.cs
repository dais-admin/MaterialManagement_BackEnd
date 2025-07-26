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
    public class MaterialWarrantyController : ControllerBase
    {
        private readonly IMaterialWarrantyService _materialWarrantyService;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialWarrantyController(IMaterialWarrantyService materialWarrantyService,
            IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialWarrantyService = materialWarrantyService;
            _materialConfig = materialConfig.Value;

        }
        [HttpGet("GetAllMaterialWarranty")]
        public async Task<IActionResult> GetAllMaterialWarranty()
        {
            var listMaterialWarranty = await _materialWarrantyService.GetAllMaterialWarranty();
            return Ok(listMaterialWarranty);
        }
        [HttpGet("GetMaterialWarrantyWithFilter")]
        public async Task<IActionResult> GetMaterialWarrantyWithFilter(string filter)
        {
            var materialFilterDto = JsonConvert.DeserializeObject<MaterialFilterDto>(filter);
            var listMaterialWarranty = await _materialWarrantyService.GetMaterialWarrantyWithFilter(materialFilterDto);
            return Ok(listMaterialWarranty);
        }
        [HttpGet("GetWarrantyById")]
        public async Task<IActionResult> GetWarrantyByIdAsync(Guid id)
        {
            var listMaterialWarranty = await _materialWarrantyService.GetWarrantyByIdAsync(id);
            return Ok(listMaterialWarranty);
        }
        [HttpGet("GetWarrantyByMaterialId")]
        public async Task<IActionResult> GetWarrantyByMaterialId(Guid materialId)
        {
            var warrantydto = await _materialWarrantyService.GetWarrantyByMaterialIdAsync(materialId);
            return Ok(warrantydto);
        }
        [HttpPost]
        public async Task<IActionResult> AddWarrantyAsync([FromForm] IFormFile? warrantyDocument, [FromForm] string warrantyData)
        {
            if (warrantyData == null || string.IsNullOrEmpty(warrantyData))
            {
                return BadRequest();
            }
            var materialWarrantyDto = JsonConvert.DeserializeObject<MaterialWarrantyDto>(warrantyData);

            if (warrantyDocument != null)
            {
                materialWarrantyDto.WarrantyDocument = await SaveWarrantyDocument(warrantyDocument, materialWarrantyDto.MaterialId);
            }
            var response = await _materialWarrantyService.AddWarrantyAsync(materialWarrantyDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWarrantyAsync([FromForm] IFormFile? warrantyDocument, [FromForm] string warrantyData)
        {
            if (warrantyData == null || string.IsNullOrEmpty(warrantyData))
            {
                return BadRequest();
            }
            var materialWarrantyDto = JsonConvert.DeserializeObject<MaterialWarrantyDto>(warrantyData);

            if (warrantyDocument != null)
            {
                materialWarrantyDto.WarrantyDocument = await SaveWarrantyDocument(warrantyDocument, materialWarrantyDto.MaterialId);
            }
            var response = await _materialWarrantyService.UpdateWarrantyAsync(materialWarrantyDto);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWarrantyAsync(Guid id)
        {
            await _materialWarrantyService.DeleteWarrantyAsync(id);
            return Ok();
        }
        [HttpGet, DisableRequestSizeLimit]
        [Route("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileUrl)
        {
            var filePath = _materialConfig.DocumentBasePath + fileUrl;
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            //Get Bytes array of your file, you can also to do a MemoryStream
            Byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            //Return your FileContentResult
            return File(bytes, "application/octet-stream", "File1");

        }
        private async Task<string> SaveWarrantyDocument([FromForm] IFormFile? warrantyDocument, Guid materialId)
        {
            var folderName = "MaterialDocument" + "\\" + materialId + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = warrantyDocument.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await warrantyDocument.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }



    }
}
