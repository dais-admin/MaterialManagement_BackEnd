using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignDocumentController : ControllerBase
    {
        private readonly IDesignDocumentService _designDocumentService;
        private readonly IFileManagerService _fileManagerService;
        private string folderName = "MaterialDocument" + "\\" + "DesignDocuments" + "\\";


        public DesignDocumentController(IDesignDocumentService designDocumentService, IFileManagerService fileManagerService)
        {
            _designDocumentService = designDocumentService;
            _fileManagerService = fileManagerService;
            
        }
        [HttpGet("GetAllDesignlDocumentAsync")]
        public async Task<IActionResult> GetAllDesignlDocumentAsync()
        {
            var listDesignlDocument = await _designDocumentService.GetAllDesignlDocumentAsync();
            return Ok(listDesignlDocument);
        }
        [HttpGet("GetDesignDocumentByIdAsync")]
        public async Task<IActionResult> GetDesignDocumentByIdAsync(Guid Id)
        {
            var listDesignDocument = await _designDocumentService.GetDesignDocumentByIdAsync(Id);
            return Ok(listDesignDocument);
        }
        [HttpPost]
        public async Task<IActionResult> AddDesignDocumentAsync(List<IFormFile> designDocuments, [FromForm] string designDocumentData)
        {
            if (designDocumentData == null || string.IsNullOrEmpty(designDocumentData))
            {
                return BadRequest();
            }
            var designDocumentDto = JsonConvert.DeserializeObject<DesignDocumentDto>(designDocumentData);
                if (designDocuments.Count > 0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var designDocument in designDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(designDocument, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                designDocumentDto.DocumentFileName =uploadedFiles.ToString();
               
            }

            var response = await _designDocumentService.AddDesignDocumentAsync(designDocumentDto);
               
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDesignDocumentAsync(DesignDocumentDto designDocumentDto)
        {
            return Ok(await _designDocumentService.UpdateDesignDocumentAsync(designDocumentDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDesignDocumentAsync(Guid Id)
        {
            await _designDocumentService.DeleteDesignDocumentAsync(Id);
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

