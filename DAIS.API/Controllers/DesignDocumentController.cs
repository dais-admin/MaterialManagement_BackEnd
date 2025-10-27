using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class DesignDocumentController : ControllerBase
    {
        private readonly IDesignDocumentService _designDocumentService;
       
        private string folderName = "MaterialDocument" + "\\" + "DesignDocuments" + "\\";
       
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IFileManagerService _fileManagerService;
        private string _filePath = string.Empty;
        private string _fileName = string.Empty;



        public DesignDocumentController(IDesignDocumentService designDocumentService, IOptions<MaterialConfigSettings> materialConfig, IFileManagerService fileManagerService)
        {
            _designDocumentService = designDocumentService;
            _materialConfig = materialConfig.Value;
            _fileManagerService = fileManagerService;

            
        }
        [HttpGet("GetAllDesignlDocumentAsync")]
        public async Task<IActionResult> GetAllDesignlDocumentAsync()
        {
            var listDesignlDocument = await _designDocumentService.GetAllDesignlDocumentAsync();
            return Ok(listDesignlDocument);
        }  
        [HttpGet("GetDesignDocumentById")]
        public async Task<IActionResult> GetDesignDocumentByIdAsync(Guid Id)
        {
            var listDesignDocument = await _designDocumentService.GetDesignDocumentByIdAsync(Id);
            return Ok(listDesignDocument);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteDesignDocumentAsync(Guid Id)
        {
            await _designDocumentService.DeleteDesignDocumentAsync(Id);
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> AddDesignDocument(List<IFormFile> documentFileName, [FromForm] string documentData)
        {
            if (documentData == null || string.IsNullOrEmpty(documentData))
            {
                return BadRequest();
            }
            var documentDto = JsonConvert.DeserializeObject<DesignDocumentDto>(documentData);
            if (documentFileName.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var designDocument in documentFileName)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(designDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                documentDto.DocumentFileName = uploadedFiles.ToString();
            }

            var response = await _designDocumentService.AddDesignDocumentAsync(documentDto);
            return Ok(response);
        }

        [HttpPut("UpdateDesignDocument")]
        public async Task<IActionResult> UpdateDesignDocumentAsync(List<IFormFile> documentFileName, [FromForm] string documentData)
        {
            if (documentData == null || string.IsNullOrEmpty(documentData))
            {
                return BadRequest();
            }
            var documentDto = JsonConvert.DeserializeObject<DesignDocumentDto>(documentData);
            if (documentFileName.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var designDocument in documentFileName)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(designDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                documentDto.DocumentFileName = uploadedFiles.ToString();
            }

            var response = await _designDocumentService.UpdateDesignDocumentAsync(documentDto);
            return Ok(response);
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

