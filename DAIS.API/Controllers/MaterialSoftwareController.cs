using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
using Dias.ExcelSteam.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialSoftwareController : ControllerBase
    {
        private readonly IMaterialSoftwareService _materialSoftwareService;
        private readonly IFileManagerService _fileManagerService;
        private string folderName = "MaterialDocument" + "\\" + "SoftwareDocuments" + "\\";

        public MaterialSoftwareController(IMaterialSoftwareService materialSoftwareService,
            IFileManagerService fileManagerService)
        {
            _materialSoftwareService = materialSoftwareService;
            _fileManagerService = fileManagerService;

            
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
        public async Task<IActionResult> AddMaterialSoftwareAsync(List<IFormFile> softwareDocuments, [FromForm] string softwareData)
        {
            if (softwareData == null || string.IsNullOrEmpty(softwareData))
            {
                return BadRequest();
            }
            var materialSoftwareDto = JsonConvert.DeserializeObject<MaterialSoftwareDto>(softwareData);          
            if (softwareDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var softwareDocument in softwareDocuments)
                {
                    folderName = folderName + materialSoftwareDto.MaterialId + "\\";
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(softwareDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                materialSoftwareDto.SoftwareDocument = uploadedFiles.ToString(); 
            }
            var response = await _materialSoftwareService.AddMaterialSoftwareAsync(materialSoftwareDto);
            return Ok(response);
        }
        [HttpPut]


        public async Task<IActionResult> UpdateMaterialSoftwareAsync(List<IFormFile> softwareDocuments, [FromForm] string softwareData)
        {
            if (softwareData == null || string.IsNullOrEmpty(softwareData))
            {
                return BadRequest();
            }
            var materialSoftwareDto = JsonConvert.DeserializeObject<MaterialSoftwareDto>(softwareData);
            if (softwareDocuments.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var softwareDocument in softwareDocuments)
                {
                    folderName = folderName + materialSoftwareDto.MaterialId + "\\";
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(softwareDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                materialSoftwareDto.SoftwareDocument = uploadedFiles.ToString();
            }
            var response = await _materialSoftwareService.UpdateMaterialSoftwareAsync(materialSoftwareDto);
            return Ok(response);
        }
        //public async Task<IActionResult> UpdateMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto)
        //{
        //    return Ok(await _materialSoftwareService.UpdateMaterialSoftwareAsync(materialSoftwareDto));
        //}
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialSoftwareAsync(Guid id)
        {
            await _materialSoftwareService.DeleteMaterialSoftwareAsync(id);
            return Ok();
        }
        
        [HttpGet("download-encrypted/{encodedPath}")]
        public async Task<IActionResult> DownloadEncrypted(string encodedPath)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedPath);
            var decodedPath = Encoding.UTF8.GetString(base64EncodedBytes);
            var (stream, isSicess) = await _fileManagerService.GetEncryptedFile(decodedPath);

            if (!isSicess)
                return NotFound("File not found.");

            return File(stream, "application/octet-stream", decodedPath);
        }
    }
}
