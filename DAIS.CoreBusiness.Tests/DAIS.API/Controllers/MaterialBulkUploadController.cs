using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialBulkUploadController : ControllerBase
    {
        private readonly IMaterialBulkUploadService _materialBulkUploadService;
        private readonly IBulkUploadDetailService _bulkUploadDetailService;
        private readonly IMaterialService _materialService;

        private readonly MaterialConfigSettings _materialConfig;
        public MaterialBulkUploadController(IMaterialBulkUploadService materialBulkUploadService,
            IBulkUploadDetailService bulkUploadDetailService,
            IMaterialService materialService,
            IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialBulkUploadService = materialBulkUploadService;
            _bulkUploadDetailService=bulkUploadDetailService;
            _materialService = materialService;
            _materialConfig = materialConfig.Value;
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
        public IActionResult BulkUpload([FromForm] IFormFile? bulkUploadFile,bool isRehabilitation)
        {
            if (bulkUploadFile == null || bulkUploadFile.Length == 0)
                return BadRequest("No file uploaded.");

            // Check file extension
            var fileExtension = Path.GetExtension(bulkUploadFile.FileName).ToLower();
            if (fileExtension != ".xlsx" && fileExtension != ".xlsm")
            {
                return BadRequest("Uploaded file must be an Excel file with .xlsx or .xlsm extension.");
            }
            var folderPath = _materialConfig.DocumentBasePath + "MaterialDocument//ExcelUploads//";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, Guid.NewGuid().ToString() + bulkUploadFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                bulkUploadFile.CopyTo(stream);
            }
            
            using var fileStream= new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);

            var response = _materialBulkUploadService.BulkUpload(fileStream, folderPath, isRehabilitation);
            return Ok(response);
        }
        [HttpGet("GetBulkUploadDetailsByUser")]
        public IActionResult GetBulkUploadDetailsByUser(string userName)
        {
            var bulkUploadDetails= _bulkUploadDetailService.GetAllBulkUploadDetailsByUser(userName);
            return Ok(bulkUploadDetails);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBulkUploadMaterials(Guid bulkUploadDetailId)
        {
            await _materialService.DeleteBulkUploadMaterials(bulkUploadDetailId);
            
            await _bulkUploadDetailService.DeleteBulkUploadDetail(bulkUploadDetailId);
            return Ok();
        }
    }
}
