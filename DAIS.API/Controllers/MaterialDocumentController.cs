using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using DAIS.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using DAIS.CoreBusiness.Services;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialDocumentController : ControllerBase
    {
        private readonly IMaterialDocumentService _materialDocumentService;
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IFileManagerService _fileManagerService;
        private string _filePath = string.Empty;
        private string _fileName = string.Empty;
        public MaterialDocumentController(IMaterialDocumentService materialDocumentService, 
            IOptions<MaterialConfigSettings> materialConfig,
            IFileManagerService fileManagerService)
        {
            _materialDocumentService = materialDocumentService;
            _materialConfig = materialConfig.Value;
            _fileManagerService = fileManagerService;
        }
        [HttpGet("GetAllMaterialDocumnet")]
        public async Task<IActionResult> GetAllMaterialDocumnet()
        {
            return Ok(await _materialDocumentService.GetAllMaterialDocumentAsync());
        }
        [HttpGet("GetMaterialDocumnetByMaterialId")]
        public async Task<IActionResult> GetMaterialDocumnetByMaterialId(Guid materialId)
        {
            return Ok(await _materialDocumentService.GetAllMaterialDocumentByMaterialIdAsync(materialId));
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> AddMaterialDocument(DocumentDto documentDto)
        {
            List<MaterialDocumentDto> materialDocumentDtoList = await SaveFile(documentDto);
            return Ok(materialDocumentDtoList);
        }
        [HttpGet("download-encrypted/{encodedPath}")]
        public async Task<IActionResult> DownloadEncrypted(string encodedPath)
        {
            // 1. Decode Base64 string to original file path
            var base64EncodedBytes = Convert.FromBase64String(encodedPath);
            var decodedPath = Encoding.UTF8.GetString(base64EncodedBytes);          
            var (stream, isSucess) = await _fileManagerService.GetEncryptedFile(decodedPath);

            if (!isSucess)
                return NotFound("File not found.");

            return File(stream, "application/octet-stream", decodedPath);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialDocument(Guid documentId)
        {
            var materialDocumentDto = await _materialDocumentService.DeleteMaterialDocumentAsync(documentId);
            return Ok();
        }

        private async Task<List<MaterialDocumentDto>> SaveFile(DocumentDto documentDto)
        {
            List<MaterialDocumentDto> materialDocumentDtoList = new List<MaterialDocumentDto>();
            var folderPath = "MaterialDocument" + "\\" + documentDto.MaterialId + "\\" + documentDto.DocumentId;
            var basePath = _materialConfig.DocumentBasePath;
            var materialDocuments = Request.Form.Files;
            foreach (var document in materialDocuments)
            {
                MaterialDocumentDto materialDocumentDto = new MaterialDocumentDto();
                _fileName = document.FileName;
                var dir = Path.Combine(basePath, folderPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (documentDto.FileAction == "Keep")
                {
                    string newFileName = document.FileName.Substring(0, document.FileName.IndexOf('.'));
                    newFileName += "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                        document.FileName.Substring(document.FileName.IndexOf('.'));
                    _fileName = newFileName;
                }
                _filePath = Path.Combine(folderPath, _fileName);
                var fullFilePath = Path.Combine(dir, _fileName);
                if (System.IO.File.Exists(fullFilePath) && documentDto.FileAction == "None")
                {
                    materialDocumentDto.ResponseMessage = "File already exists with same name";
                    materialDocumentDto.IsSuccess = false;
                }
                else
                {
                      var (isSucces,savedFile)=  await _fileManagerService.UploadAndEncryptFile(document, dir);
                     
                    if (documentDto.FileAction == "None" || documentDto.FileAction == "Keep")
                    {
                        materialDocumentDto = await SaveFileMetaData(documentDto);
                    }

                    materialDocumentDto.ResponseMessage = "File Uploaded Successfully";
                    materialDocumentDto.IsSuccess = true;

                }
                materialDocumentDtoList.Add(materialDocumentDto);
            }
            return materialDocumentDtoList;
        }
        private async Task<MaterialDocumentDto> SaveFileMetaData(DocumentDto documentDto)
        {
            MaterialDocumentDto materialDocumentDto = new MaterialDocumentDto();
            materialDocumentDto.DocumentFileName = _fileName;
            materialDocumentDto.DocumentFilePath = _filePath;
            materialDocumentDto.MaterialId = documentDto.MaterialId;
            materialDocumentDto.DocumentTypeId = documentDto.DocumentId;
            materialDocumentDto = await _materialDocumentService.AddMaterialDocumentAsync(materialDocumentDto);
            return materialDocumentDto;

        }
       
        
        
    }
}
