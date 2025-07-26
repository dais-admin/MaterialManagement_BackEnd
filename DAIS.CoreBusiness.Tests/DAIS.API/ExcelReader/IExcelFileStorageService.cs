namespace DAIS.API.ExcelReader
{
    public interface IExcelFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task DeleteFileAsync(string filePath);
    }
}
