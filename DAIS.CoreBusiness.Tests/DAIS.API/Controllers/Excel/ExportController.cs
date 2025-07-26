using DAIS.API.ExcelReader;
using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Data;
using Dias.ExcelSteam.Queue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DAIS.API.Controllers.Excel
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly IExcelFileStorageService _fileStorageService;
        private readonly IMaterialBulkUploadService _materialBulkUploadService;
        private readonly MaterialConfigSettings _materialConfig;
        public ExportController(IBackgroundTaskQueue backgroundTaskQueue,
            IServiceProvider serviceProvider,
            IExcelFileStorageService fileStorageService,
            IMaterialBulkUploadService materialBulkUploadService,
            IOptions<MaterialConfigSettings> materialConfig)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceProvider = serviceProvider;
            _fileStorageService = fileStorageService;
            _materialBulkUploadService= materialBulkUploadService;
            _materialConfig= _materialConfig = materialConfig.Value;
        }
        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadTemplate")]
        public async Task<IActionResult> DownloadTemplate([FromQuery] string fileUrl)
        {
            var filePath = _materialConfig.DocumentBasePath + fileUrl;
            if (!System.IO.File.Exists(filePath))
            {
                var folderPath = _materialConfig.DocumentBasePath + "Templates//";

                await _materialBulkUploadService.GenrateMaterialUploadTemplate(folderPath);
            }

            //Get Bytes array of your file, you can also to do a MemoryStream
            Byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            //Return your FileContentResult
            return File(bytes, "application/octet-stream", "File1");

        }
        [HttpPost]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile? bulkUploadFile)
        {
            BulkUploadResponseDto bulkUploadResponseDto = new BulkUploadResponseDto();
            if (bulkUploadFile == null || bulkUploadFile.Length == 0)
                return BadRequest("No file uploaded.");

            // Check file extension
            var fileExtension = Path.GetExtension(bulkUploadFile.FileName).ToLower();
            if (fileExtension != ".xlsx" && fileExtension != ".xlsm")
            {
                return BadRequest("Uploaded file must be an Excel file with .xlsx or .xlsm extension.");
            }

            try
            {
                var filePath = await _fileStorageService.SaveFileAsync(bulkUploadFile);

                await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                {
                    try
                    {
                        using var streamForBackground = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        using var scope = _serviceProvider.CreateScope();
                        var importService = scope.ServiceProvider.GetRequiredService<IExcelDataImporter>();
                        var records = await importService.ImportExcelDataAsync(streamForBackground, token).ConfigureAwait(false);

                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        await context.ExcelReaderMetadata.AddRangeAsync(records, token).ConfigureAwait(false);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error processing file: {ex.Message}");
                        bulkUploadResponseDto.ResponseMessage= ex.Message;
                        bulkUploadResponseDto.IsSuccess = false;
                    }
                    finally
                    {
                        // Delete the file after processing
                        await _fileStorageService.DeleteFileAsync(filePath);
                    }
                });
                bulkUploadResponseDto.ResponseMessage = "Bulk Upload is in progress";
                bulkUploadResponseDto.IsSuccess=true;
                return Ok(bulkUploadResponseDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
    }
}


