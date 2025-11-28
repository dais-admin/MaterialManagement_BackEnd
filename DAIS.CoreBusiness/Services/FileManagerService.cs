using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace DAIS.CoreBusiness.Services
{
    public class FileManagerService: IFileManagerService
    {
        private readonly string _rootFolder=string.Empty;
        private readonly byte[] Key;
        private readonly byte[] IV;
        private readonly IOptions<FileSettings> _fileSettings;
        private readonly ILogger<FileManagerService> _logger;
        public FileManagerService(IOptions<FileSettings> fileSettings,
            ILogger<FileManagerService> logger)
            {
            _fileSettings = fileSettings;
            _rootFolder = _fileSettings.Value.RootDirectory;
            _logger= logger;
            if (!Directory.Exists(_rootFolder))
            {
                Directory.CreateDirectory(_rootFolder);
            }

            this.Key= Encoding.UTF8.GetBytes(_fileSettings.Value.FileEncryptionKey); // 32 bytes
            this.IV = Encoding.UTF8.GetBytes(_fileSettings.Value.FileEncryptionIV); // 16 bytes
        }
        public async Task<(bool,string)> UploadFiles(List<IFormFile> files,string path)
        {
            bool isSuccess = true;
            StringBuilder uploadedFilePath = new StringBuilder();
            try
            {
                if (files == null || files.Count == 0)
                    return (false,string.Empty);
                if (!Directory.Exists(_rootFolder))
                {
                    Directory.CreateDirectory(_rootFolder);
                }
                string folderPath = Path.Combine(_rootFolder, path);
                foreach (IFormFile file in files)
                {
                    var filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    uploadedFilePath.Append(Path.Combine(path, file.FileName));
                    uploadedFilePath.Append(";");
                }              
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return (isSuccess, uploadedFilePath.ToString());
        }

        public void Delete(string relativeFilePath)
        {
            try
            {
                string fullPath = Path.Combine(_rootFolder, relativeFilePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation($"Deleted file: {fullPath}");
                }
                else
                {
                    _logger.LogWarning($"File not found: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to delete file: {relativeFilePath}");
                throw;
            }
        }


        public async Task<(bool, string)> UploadAndEncryptFile(IFormFile file, string path)
        {
            bool isSuccess = true;
            StringBuilder uploadedFilePath = new StringBuilder();
            try
            {
                if (file == null || file.Length == 0)
                    return (false, string.Empty);
                
                string folderPath = Path.Combine(_rootFolder, path);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath= Path.Combine(folderPath, Path.GetFileName(file.FileName));
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                EncryptAndSaveFile(memoryStream, filePath);
                uploadedFilePath.Append(Path.Combine(path, Path.GetFileName(file.FileName)));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while uploading  Encrypt File:"+ex.Message, ex);
                isSuccess = false;
                throw ex;             
            }

            return (isSuccess, uploadedFilePath.ToString());
        }
        public async Task<(Stream, bool)> GetFile(string fileName)
        {
            var filePath = Path.Combine(_rootFolder, fileName);

            if (!File.Exists(filePath))
                return (null,false);

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var contentType = "application/octet-stream"; // Or derive from extension

            return (stream,true);
        }

        public async Task<(Stream, bool)> GetEncryptedFile(string fileName)
        {
            var filePath = Path.Combine(_rootFolder, fileName);

            if (!File.Exists(filePath))
                return (null, false);

            var stream = DecryptFileToStream(filePath);
            return (stream, true);
        }
        public void EncryptAndSaveFile(Stream inputStream, string filePath)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            using var cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write);

            inputStream.CopyTo(cryptoStream);
        }

        public  Stream DecryptFileToStream(string filePath)
        {
            var outputStream = new MemoryStream();
            try
            {
                using var aes = Aes.Create();
                aes.Key = Key;
                aes.IV = IV;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var fileStream = new FileStream(filePath, FileMode.Open);
                using var cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read);

                cryptoStream.CopyTo(outputStream);
                outputStream.Position = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While DecryptFile:"+ex.Message, ex);
                throw ex;
            }
            return outputStream;
        }

        /*[HttpPost("upload-encrypted")]
        public async Task<IActionResult> UploadEncrypted(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var filePath = Path.Combine(_secureFolder, Path.GetFileName(file.FileName));
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            FileEncryptionHelper.EncryptAndSaveFile(memoryStream, filePath);
            return Ok("Encrypted file uploaded.");
        }

        [HttpGet("download-encrypted/{fileName}")]
        public IActionResult DownloadEncrypted(string fileName)
        {
            var filePath = Path.Combine(_secureFolder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var stream = FileEncryptionHelper.DecryptFileToStream(filePath);
            return File(stream, "application/octet-stream", fileName);
        }*/
    }
}
