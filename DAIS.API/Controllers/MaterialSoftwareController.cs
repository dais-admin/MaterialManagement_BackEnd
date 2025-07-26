using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
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
    public class MaterialSoftwareController : ControllerBase
    {
        private readonly IMaterialSoftwareService _materialSoftwareService;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialSoftwareController(IMaterialSoftwareService materialSoftwareService, 
            IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialSoftwareService = materialSoftwareService;
            _materialConfig=materialConfig.Value;

            
        }
        [HttpGet("GetAllMaterialSoftware")]
        public async Task<IActionResult> GetAllMaterialSoftware()
        {
            var listMaterialSoftware = await _materialSoftwareService.GetAllMaterialSoftware();
            return Ok(listMaterialSoftware);
        }
        [HttpGet("GetSoftwareListByMaterialId")]
        public async Task<IActionResult> GetSoftwareListByMaterialId(Guid materialId)
        {
            var listMaterialSoftware = await _materialSoftwareService.GetSoftwareListByMaterialIdAsync(materialId);
            return Ok(listMaterialSoftware);
        }
        [HttpGet("GetMaterialSoftwareById")]
        public async Task<IActionResult> GetMaterialSoftwareById(Guid id)
        {
            var listMaterialSoftware = await _materialSoftwareService.GetMaterialSoftwareByIdAsync(id);
            return Ok(listMaterialSoftware);
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterialSoftwareAsync([FromForm] IFormFile? softwareDocuments, [FromForm] string softwareData)
        {
            if (softwareData == null || string.IsNullOrEmpty(softwareData))
            {
                return BadRequest();
            }
            var materialSoftwareDto = JsonConvert.DeserializeObject<MaterialSoftwareDto>(softwareData);
            var softwareDocumentFiles = Request.Form.Files;
            if (softwareDocumentFiles != null)
            {
                string documentFiles = string.Empty;
                foreach( var softwareDocument in softwareDocumentFiles)
                {
                    documentFiles = documentFiles + 
                        await SaveSoftwareDocument(softwareDocument, materialSoftwareDto.MaterialId)+";";
                }
                materialSoftwareDto.SoftwareDocument = documentFiles;
            }
            var response = await _materialSoftwareService.AddMaterialSoftwareAsync(materialSoftwareDto);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto)
        {
            return Ok(await _materialSoftwareService.UpdateMaterialSoftwareAsync(materialSoftwareDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialSoftwareAsync(Guid id)
        {
            await _materialSoftwareService.DeleteMaterialSoftwareAsync(id);
            return Ok();
        }
        private async Task<string> SaveSoftwareDocument([FromForm] IFormFile? documentFile, Guid materialId)
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
