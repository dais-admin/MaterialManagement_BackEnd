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
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IFileManagerService _fileManagerService;
        private readonly string folderName = "MaterialDocument" + "\\" + "ManufacturerDocument" + "\\";

        public ManufacturerController(IManufacturerService manufacturerService,
            IFileManagerService fileManagerService)
        {
            _manufacturerService = manufacturerService;
            _fileManagerService = fileManagerService;

        }
        [HttpGet("GetAllManufacturer")]
        public async Task<IActionResult> GetAllManufacturer()
        {
            var listManufacturer = await _manufacturerService.GetAllManufacturer();
            return Ok(listManufacturer);
        }
        [HttpGet("GetManufacturer")]
        public async Task<IActionResult> GetManufacturer(Guid id)
        {
            var listManufacturer = await _manufacturerService.GetManufacturer(id);
            return Ok(listManufacturer);
        }
        [HttpPost]
        public async Task<IActionResult> AddManufacturer(List<IFormFile> manufacturerDocuments, [FromForm] string manufacturerData)
        {
            if (manufacturerData == null || string.IsNullOrEmpty(manufacturerData))
            {
                return BadRequest();
            }
            var ManufacturerDto = JsonConvert.DeserializeObject<ManufacturerDto>(manufacturerData);
            if (manufacturerDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var manufacturerDocument in manufacturerDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(manufacturerDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                ManufacturerDto.ManufacturerDocument = uploadedFiles.ToString();
            }
            
            var response = await _manufacturerService.AddManufacturer(ManufacturerDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateManufacturer(List<IFormFile> manufacturerDocuments, [FromForm] string manufacturerData)
        {
            if (manufacturerData == null || string.IsNullOrEmpty(manufacturerData))
            {
                return BadRequest();
            }
            var ManufacturerDto = JsonConvert.DeserializeObject<ManufacturerDto>(manufacturerData);
            if (manufacturerDocuments != null)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var manufacturerDocument in manufacturerDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(manufacturerDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                ManufacturerDto.ManufacturerDocument = uploadedFiles.ToString();
            }
            var response = await _manufacturerService.UpdateManufactuter(ManufacturerDto);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteManufacturer(Guid id)
        {
            await _manufacturerService.DeleteManufacturer(id);
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
