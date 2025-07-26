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
    public class MaterialHardwareController : ControllerBase
    {
        private readonly IMaterialHardwareService _materialHardwareService;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialHardwareController(IMaterialHardwareService materialHardwareService,
            IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialHardwareService = materialHardwareService;
            _materialConfig = materialConfig.Value;
        }
        [HttpGet("GetAllMaterialHardware")]
        public async Task<IActionResult> GetAllMaterialHardware()
        {
            var listMaterialHardware = await _materialHardwareService.GetAllMaterialHardware();
            return Ok(listMaterialHardware);
        }
        [HttpGet("GetAllMaterialHardwareByMaterialCode")]
        public async Task<IActionResult> GetAllMaterialHardwareByMaterialCode(Guid materialId)
        {
            var listMaterialHardware = await _materialHardwareService.GetAllMaterialHardwaresByMaterialId(materialId);
            return Ok(listMaterialHardware);
        }
        [HttpGet("GetMaterialHardwareById")]
        public async Task<IActionResult> GetMaterialHardwareById(Guid id)
        {
            var listMaterialHardware = await _materialHardwareService.GetMaterialHardwareByIdAsync(id);
            return Ok( listMaterialHardware);
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterialHardwareAsync([FromForm] IFormFile? hardwareDocuments, [FromForm] string hardwareData)
        {
            if (hardwareData == null || string.IsNullOrEmpty(hardwareData))
            {
                return BadRequest();
            }
            var materialHardwareDto = JsonConvert.DeserializeObject<MaterialHardwareDto>(hardwareData);

            var hardwareDocumentFiles = Request.Form.Files;
            if (hardwareDocumentFiles != null)
            {
                string documentFiles = string.Empty;
                foreach (var hardwareDocument in hardwareDocumentFiles)
                {
                    documentFiles = documentFiles +
                        await SaveHardwareDocument(hardwareDocument, materialHardwareDto.MaterialId) + ";";
                }
                if (!string.IsNullOrEmpty(documentFiles))
                {
                    materialHardwareDto.HardwareDocument = documentFiles;
                }
               
            }

            var response =await _materialHardwareService.AddMaterialHardwareAsync(materialHardwareDto);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterialHardwareAsync([FromForm] IFormFile? hardwareDocuments, [FromForm] string hardwareData)
        {
            if (hardwareData == null || string.IsNullOrEmpty(hardwareData))
            {
                return BadRequest();
            }
            var materialHardwareDto = JsonConvert.DeserializeObject<MaterialHardwareDto>(hardwareData);
            var hardwareDocumentFiles = Request.Form.Files;
            if (hardwareDocumentFiles != null)
            {
                string documentFiles = string.Empty;
                foreach (var hardwareDocument in hardwareDocumentFiles)
                {
                    documentFiles = documentFiles +
                        await SaveHardwareDocument(hardwareDocument, materialHardwareDto.MaterialId) + ";";
                }
                materialHardwareDto.HardwareDocument = documentFiles;
            }
            
            var response = await _materialHardwareService.UpdateMaterialHardwareAsync(materialHardwareDto);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialHardwareAsync(Guid id)
        {
            await _materialHardwareService.DeleteMaterialHardwareAsync(id);
            return Ok();
        }

        private async Task<string> SaveHardwareDocument([FromForm] IFormFile? documentFile, Guid materialId)
        {
            var folderName = "MaterialDocument" + "\\" + materialId + "\\";
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
