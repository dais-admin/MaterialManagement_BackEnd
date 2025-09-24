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
    public class MaterialMaintenanceController : ControllerBase
    {
        private readonly IMaterialMaintenanceService _materialMaintenanceService;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialMaintenanceController(IMaterialMaintenanceService materialMaintenanceService,
             IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialMaintenanceService = materialMaintenanceService;
            _materialConfig = materialConfig.Value;

        }
        [HttpGet("GetAllMaterialMaintenance")]
        public async Task<IActionResult> GetAllMaterialMaintenacesAsync()
        {
            var listMaterialMaintenance = await _materialMaintenanceService.GetAllMaterialMaintenacesAsync();
            return Ok(listMaterialMaintenance);
        }
        [HttpGet("GetMaterialMaintenanceById")]
        public async Task<IActionResult> GetMaterialMaintenaceByIdAsync(Guid id)
        {
            var listMaterialMaintenance = await _materialMaintenanceService.GetMaterialMaintenaceByIdAsync(id);
            return Ok(listMaterialMaintenance);
        }
        [HttpGet("GetMaintenanceByMaterialIdAsync")]
        public async Task<IActionResult> GetMaintenanceByMaterialIdAsync(Guid materialId)
        {
            var maintenanceDto = await _materialMaintenanceService.GetMaintenanceByMaterialIdAsync(materialId);
            return Ok(maintenanceDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterialMaintenanceAsync([FromForm] IFormFile? maintenanceDocument, [FromForm] string maintenanceData)
        {
            if (maintenanceData == null || string.IsNullOrEmpty(maintenanceData))
            {
                return BadRequest();
            }
            var maintenanceDto = JsonConvert.DeserializeObject<MaterialMaintenaceDto>(maintenanceData);

            if (maintenanceDocument != null)
            {
                maintenanceDto.MaintenanceDocument = await SaveMaintenanceDocument(maintenanceDocument);
            }
            var response = await _materialMaintenanceService.AddMaterialMaintenanceAsync(maintenanceDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMaterialMaintenaceAsync([FromForm] IFormFile? maintenanceDocument, [FromForm] string maintenanceData)
        {
            if (maintenanceData == null || string.IsNullOrEmpty(maintenanceData))
            {
                return BadRequest();
            }
            var maintenanceDto = JsonConvert.DeserializeObject<MaterialMaintenaceDto>(maintenanceData);

            if (maintenanceDocument != null)
            {
                maintenanceDto.MaintenanceDocument = await SaveMaintenanceDocument(maintenanceDocument);
            }
            var response = await _materialMaintenanceService.UpdateMaterialMaintenaceAsync(maintenanceDto);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialMaintenaceAsync(Guid id)
        {
            await _materialMaintenanceService.DeleteMaterialMaintenaceAsync(id);
            return Ok();
        }

        private async Task<string> SaveMaintenanceDocument([FromForm] IFormFile? documentFile)
        {
            var folderName = "MaterialDocument" + "\\" + "MaintenanceDocuments" + "\\";
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

