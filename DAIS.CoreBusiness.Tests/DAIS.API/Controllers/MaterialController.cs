using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly MaterialConfigSettings _materialConfig;      
        public MaterialController(IMaterialService materialService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialService = materialService;
            _materialConfig = materialConfig.Value;
        }
        [HttpGet("GetMaterialById")]
        public async Task<IActionResult> GetMaterialById(Guid id)
        {
            return Ok(await _materialService.GetMaterialByIdAsync(id));
        }
        [HttpGet("GetMaterialByCode")]
        public async Task<IActionResult> GetMaterialByCode(string materialCode)
        {
            var response = await _materialService.GetMaterialsByCodeAsync(materialCode);
            return Ok(response);
        }
        [HttpGet("GetAllMaterials")]
        public async Task<IActionResult> GetAllMaterials(Guid workPackageId)
        {
            return Ok(await _materialService.GetAllMaterialsAsync(workPackageId));
        }
        [HttpGet("GetAllMaterialsByLocation")]
        public async Task<IActionResult> GetAllMaterialsByLocation(Guid locationId)
        {
            return Ok(await _materialService.GetAllMaterialsByLocationAsync(locationId));
        }
        [HttpGet("GetAllMaterialsByDivision")]
        public async Task<IActionResult> GetAllMaterialsByDivision(Guid divisionId)
        {
            return Ok(await _materialService.GetAllMaterialsByDivisionAsync(divisionId));
        }
        [HttpGet("GetAllMaterialsBySubDivision")]
        public async Task<IActionResult> GetAllMaterialsBySubDivision(Guid subdivisionId)
        {
            return Ok(await _materialService.GetAllMaterialsBySubDivisionAsync(subdivisionId));
        }
        [HttpGet("GetAllMaterialsBySystem")]
        public async Task<IActionResult> GetAllMaterialsBySystem(string systemName,bool isRehabilitation)
        {

            var response = await _materialService.GetMaterialsBySystemAsync(systemName,isRehabilitation);
            return Ok(response);
        }
        [HttpGet("getAllMaterialListWithFilter")]
        public async Task<IActionResult> getAllMaterialListWithFilter(string filters)
        {
            var materialFilterDto = JsonConvert.DeserializeObject<MaterialFilterDto>(filters);
            var response = await _materialService.GetMaterialsWithFilterAsync(materialFilterDto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterial([FromForm] IFormFile? materialImage, [FromForm] string materialData)
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
            
            if (materialImage!=null)
            {
                materialDto.MaterialImage = await SaveMaterialImage(materialImage, materialDto.MaterialCode);
            }
            var savedMaterialDto = await _materialService.AddMaterialAsync(materialDto);
            return Ok(savedMaterialDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterial([FromForm] IFormFile? materialImage, [FromForm] string materialData)
        {
            if (materialData == null || string.IsNullOrEmpty(materialData))
            {
                return BadRequest();
            }
            var materialDto = JsonConvert.DeserializeObject<MaterialDto>(materialData);
            if (materialImage != null)
            {
                materialDto.MaterialImage = await SaveMaterialImage(materialImage, materialDto.MaterialCode);
            }
            var updatedMaterialDto = await _materialService.UpdateMaterialAsync(materialDto);
            return Ok(updatedMaterialDto);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            await _materialService.DeleteMaterialAsync(id);
            return Ok();
        }
        [HttpGet("GetAllMaterialMasters")]
        public async Task<IActionResult> GetAllAssetMasters()
        {
            return Ok(await _materialService.GetAllMaterialMasters());  
        }
        [HttpGet("GetMaterialAuditReport")]
        public async Task<IActionResult> GetMaterialAuditReport()
        {
            var response =await _materialService.GetMaterialAuditReport();
            return Ok(response);
        }

        private async Task<string> SaveMaterialImage([FromForm] IFormFile? materialImage,string materialCode)
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
