using Microsoft.AspNetCore.Http;

namespace DAIS.Infrastructure.Cryptography
{
    public interface IFileEncryptionService
    {
        Task EncryptFileAsync(IFormFile file, string filePath);
       // Task EncryptFileAsync(string inputFilePath, string outputFilePath);
        Task DecryptFileAsync(string inputFilePath, string outputFilePath);
    }
}
