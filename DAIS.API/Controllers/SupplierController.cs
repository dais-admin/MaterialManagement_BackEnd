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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;    
        private readonly IFileManagerService _fileManagerService;
        private readonly string folderName = "MaterialDocument" + "\\" + "SupplierDocuments" + "\\";
        public SupplierController(ISupplierService supplierService,             
            IFileManagerService fileManagerService)
        {
            _supplierService = supplierService;
            _fileManagerService = fileManagerService;
        }
        [HttpGet("GetAllSupplier")]
        public async Task<IActionResult> GetAllSupplier()
        {
            var listSupplier = await _supplierService.GetAllSupplier();
            return Ok(listSupplier);
        }
        [HttpGet("GetSupplier")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var listSupplier = await _supplierService.GetSupplier(id);
            return Ok(listSupplier);
        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier(List<IFormFile> supplierDocuments, [FromForm] string supplierData)
        {
            if (supplierData == null || string.IsNullOrEmpty(supplierData))
            {
                return BadRequest();
            }
            var supplierDto = JsonConvert.DeserializeObject<SupplierDto>(supplierData);
            if (supplierDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var supplierDocumentFile in supplierDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(supplierDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                supplierDto.SupplierDocument = uploadedFiles.ToString();
            }
            
            var response = await _supplierService.AddSupplier(supplierDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSupplier(List<IFormFile> supplierDocuments, [FromForm] string supplierData)
        {
            if (supplierData == null || string.IsNullOrEmpty(supplierData))
            {
                return BadRequest();
            }
            var supplierDto = JsonConvert.DeserializeObject<SupplierDto>(supplierData);
            if (supplierDocuments.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var supplierDocumentFile in supplierDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(supplierDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                supplierDto.SupplierDocument = uploadedFiles.ToString();
            }

            var response = await _supplierService.UpdateSupplier(supplierDto);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            await _supplierService.DeleteSupplier(id);
            return Ok();
        }

        [HttpGet("download-encrypted/{encodedPath}")]
        public async Task<IActionResult> DownloadEncrypted(string encodedPath)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedPath);
            var decodedPath = Encoding.UTF8.GetString(base64EncodedBytes);          
            var (stream,isSicess) = await _fileManagerService.GetEncryptedFile(decodedPath);

            if (!isSicess)
                return NotFound("File not found.");

            return File(stream, "application/octet-stream", decodedPath);
        }
       
    }
}
