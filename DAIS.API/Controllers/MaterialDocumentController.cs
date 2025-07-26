using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using DAIS.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialDocumentController : ControllerBase
    {
        private readonly IMaterialDocumentService _materialDocumentService;
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IFileEncryptionService _fileEncryptionService;

        private string _filePath = string.Empty;
        private string _fileName = string.Empty;
        public MaterialDocumentController(IMaterialDocumentService materialDocumentService, IOptions<MaterialConfigSettings> materialConfig, IFileEncryptionService fileEncryptionService)
        {
            _materialDocumentService = materialDocumentService;
            _materialConfig = materialConfig.Value;
            _fileEncryptionService = fileEncryptionService;
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
        [HttpGet, DisableRequestSizeLimit]
        [Route("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileUrl)
        {
            var filePath = _materialConfig.DocumentBasePath + fileUrl;
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            //Get Bytes array of your file, you can also to do a MemoryStream
            //Byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var decryptedFilePath = Path.Combine("decrypted_files", Path.GetFileName(filePath));

            // Decrypt file
            await _fileEncryptionService.DecryptFileAsync(filePath, decryptedFilePath);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(decryptedFilePath);
            System.IO.File.Delete(decryptedFilePath); // Clean up the decrypted file
            //Return your FileContentResult
            return File(fileBytes, "application/octet-stream", "File1");

        }
        [HttpGet("download/{fileUrl}")]
        public async Task<IActionResult> Download(string fileUrl)
        {
            //Or We can impleplies  API/method with dbcontenxt, need to be pass id and get all details of the file
            var filePath = _materialConfig.DocumentBasePath + fileUrl;

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var decryptedFilePath = Path.Combine("decrypted_files", Path.GetFileName(filePath));

            // Decrypt file
            await _fileEncryptionService.DecryptFileAsync(filePath, decryptedFilePath);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(decryptedFilePath);
            System.IO.File.Delete(decryptedFilePath); // Clean up the decrypted file

            return File(fileBytes, "application/octet-stream", "FileName");
        }
        [HttpPost("upload")]
        public async Task<IActionResult> AddMaterialDocument(DocumentDto documentDto)
        {
            List<MaterialDocumentDto> materialDocumentDtoList = await SaveFile(documentDto);
            return Ok(materialDocumentDtoList);
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
                   //await _fileEncryptionService.EncryptFileAsync(document, fullFilePath).ConfigureAwait(false);

                    await document.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));
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
       
        
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }
}
