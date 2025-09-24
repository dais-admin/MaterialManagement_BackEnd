using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialServiceProviderController : ControllerBase
    {
        private readonly IMaterialServiceProviderService _materialServiceProvider;
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IFileManagerService _fileManagerService;
        private readonly string folderName = "MaterialDocument" + "\\" + "ServiceProviderDocument" + "\\";
        public MaterialServiceProviderController(IMaterialServiceProviderService materialServiceProvider, IFileManagerService fileManagerService)
        {
            _materialServiceProvider = materialServiceProvider;
            _fileManagerService = fileManagerService;

        }
        [HttpGet("GetAllMaterialServiceProvides")]
        public async Task<IActionResult> GetAllMaterialServiceProvides()
        {
            var listMaterialService = await _materialServiceProvider.GetAllServiceProviderAsync();
            return Ok(listMaterialService);
        }
        [HttpGet("GetMaterialServiceProviderById")]
        public async Task<IActionResult> GetMaterialServiceProviderById(Guid id)
        {
            var listMaterialService = await _materialServiceProvider.GetServiceProviderByIdAsync(id);
            return Ok(listMaterialService);
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceProviderAsync(List<IFormFile> serviceProviderDocument, [FromForm] string serviceProviderData)
        {
            if (serviceProviderData == null || string.IsNullOrEmpty(serviceProviderData))
            {
                return BadRequest();
            }
            var serviceProviderDto = JsonConvert.DeserializeObject<MaterialServiceProviderDto>(serviceProviderData);

            if (serviceProviderDocument.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                string documentFiles = string.Empty;
                foreach (var serviceProviderDocumentDocumentFile in serviceProviderDocument)
                {
                    var (isSuccess, savedFile) = await _fileManagerService.UploadAndEncryptFile(serviceProviderDocumentDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");

                }
                serviceProviderDto.ServiceProviderDocument = uploadedFiles.ToString();
            }
            var response = await _materialServiceProvider.AddServiceProviderAsync(serviceProviderDto);
            return Ok(response);
        }


        [HttpPut]

        public async Task<IActionResult> UpdateServiceProviderAsync(List<IFormFile> serviceProviderDocument, [FromForm] string serviceProviderData)
        {
            if (serviceProviderData == null || string.IsNullOrEmpty(serviceProviderData))
            {
                return BadRequest();
            }
            var serviceProviderDto = JsonConvert.DeserializeObject<MaterialServiceProviderDto>(serviceProviderData);

            if (serviceProviderDocument.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                string documentFiles = string.Empty;
                foreach (var serviceProviderDocumentDocumentFile in serviceProviderDocument)
                {
                    var (isSuccess, savedFile) = await _fileManagerService.UploadAndEncryptFile(serviceProviderDocumentDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");

                }
                serviceProviderDto.ServiceProviderDocument = uploadedFiles.ToString();
            }
            var response = await _materialServiceProvider.UpdateServiceProviderAsync(serviceProviderDto);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteServiceProviderAsync(Guid id)
        {
            await _materialServiceProvider.DeleteServiceProviderAsync(id);
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
