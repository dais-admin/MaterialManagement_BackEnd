using DAIS.DataAccess.Entities;

namespace DAIS.API.ExcelReader
{
    public interface IExcelDataImporter
    {
        Task<List<ExcelReaderMetadata>> ImportExcelDataAsync(Stream fileStream, CancellationToken token);
    }
}
