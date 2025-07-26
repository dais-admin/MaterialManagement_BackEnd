using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace DAIS.Infrastructure.Cryptography
{
    public class FileEncryptionService : IFileEncryptionService
    {
        private readonly byte[] key;
        private readonly byte[] iv;
        public FileEncryptionService()
        {
            string keyBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
            string ivBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_IV");

            if (string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
            {
                throw new InvalidOperationException("Encryption key or IV is not set in environment variables.");
            }

            key = Convert.FromBase64String(keyBase64);
            iv = Convert.FromBase64String(ivBase64);

            if (key.Length != 32 || iv.Length != 16) 
            {
                throw new InvalidOperationException("Invalid key or IV length.");
            }
        }
        public async Task EncryptFileAsync(IFormFile file,string filePath)
        {

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var inputFileStream = file.OpenReadStream())
                    using (var outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (var cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                    {
                        await inputFileStream.CopyToAsync(cryptoStream);
                    }
                }
            }
           
        }

        public async Task DecryptFileAsync(string inputFilePath, string outputFilePath)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                        using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        using (var cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                        {
                            await cryptoStream.CopyToAsync(outputFileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}


