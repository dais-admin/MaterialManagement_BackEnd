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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly MaterialConfigSettings _materialConfig;
        public InventoryController(IInventoryService inventoryService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _inventoryService=inventoryService;
            _materialConfig = materialConfig.Value;
        }
        [HttpGet("GetMaterialInventoryByCode")]
        public async Task<IActionResult> GetMaterialInventoryByCode(string materialCode)
        {
            var response = await _inventoryService.GetMaterialInventoryByCodeAsync(materialCode);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterialInventory([FromForm] IFormFile? materialDocument, [FromForm] string materialData)
        {
            if (materialData == null || string.IsNullOrEmpty(materialData))
            {
                return BadRequest();
            }
            var materialDto = JsonConvert.DeserializeObject<MaterialDto>(materialData);
            if (materialDto == null)
            {
                return BadRequest();
            }

            if (materialDocument != null)
            {
                materialDto.MaterialImage = await SaveMaterialImage(materialDocument, materialDto.MaterialCode);
            }
            var updatedMaterialDto = await _inventoryService.UpdateMaterialAsync(materialDto);
            return Ok(updatedMaterialDto);
        }
        private async Task<string> SaveMaterialImage([FromForm] IFormFile? materialImage, string materialCode)
        {
            var folderName = "MaterialDocument" + "\\" + materialCode + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = materialImage.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await materialImage.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }
    }
}
