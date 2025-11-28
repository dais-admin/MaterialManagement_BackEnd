using Microsoft.AspNetCore.Http;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IFileManagerService
    {
        Task<(bool, string)> UploadAndEncryptFile(IFormFile file, string path);
        Task<(Stream, bool)> GetEncryptedFile(string fileName);
        void Delete(string relativeFilePath);

    }
}
