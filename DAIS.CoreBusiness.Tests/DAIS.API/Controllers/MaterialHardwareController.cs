using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
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
    public class MaterialHardwareController : ControllerBase
    {
        private readonly IMaterialHardwareService _materialHardwareService;
        private readonly IFileManagerService _fileManagerService;
        private string folderName = "MaterialDocument" + "\\" + "HardwareDocument" + "\\";

        public MaterialHardwareController(IMaterialHardwareService materialHardwareService,
            IFileManagerService fileManagerService)
        {
            _materialHardwareService = materialHardwareService;
            _fileManagerService = fileManagerService;
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
        public async Task<IActionResult> AddMaterialHardwareAsync(List<IFormFile> hardwareDocuments, [FromForm] string hardwareData)
        {
            if (hardwareData == null || string.IsNullOrEmpty(hardwareData))
            {
                return BadRequest();
            }
            var materialHardwareDto = JsonConvert.DeserializeObject<MaterialHardwareDto>(hardwareData);
            if (hardwareDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var hardwareDocument in hardwareDocuments)
                {
                    folderName = folderName + materialHardwareDto.MaterialId + "\\";
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(hardwareDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                materialHardwareDto.HardwareDocument = uploadedFiles.ToString();

            }

            var response =await _materialHardwareService.AddMaterialHardwareAsync(materialHardwareDto);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterialHardwareAsync(List<IFormFile> hardwareDocuments, [FromForm] string hardwareData)
        {
            if (hardwareData == null || string.IsNullOrEmpty(hardwareData))
            {
                return BadRequest();
            }
            var materialHardwareDto = JsonConvert.DeserializeObject<MaterialHardwareDto>(hardwareData);
            if (hardwareDocuments.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var hardwareDocument in hardwareDocuments)
                {
                    folderName = folderName + materialHardwareDto.MaterialId + "\\";
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(hardwareDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                materialHardwareDto.HardwareDocument = uploadedFiles.ToString();

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
