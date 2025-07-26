namespace DAIS.API.ExcelReader
{
    public class ExcelFileStorageService : IExcelFileStorageService
    {
        private readonly string _directoryPath;
        public ExcelFileStorageService(IConfiguration configuration)
        {
            _directoryPath = Path.Combine(configuration["MaterialConfig:DocumentBasePath"]!, "ExcelUploads");
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            await Task.FromResult(0);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(_directoryPath, Guid.NewGuid().ToString() + file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
