using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Interfaces.Report;
using DAIS.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMaterialAuditReportService _materialAuditReportService;
        private readonly IMaterialAuditService _materialAuditService;
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IFileEncryptionService _fileEncryptionService;

        public ReportsController(
            IMaterialAuditReportService materialAuditReport,
            IMaterialAuditService materialAuditService,
            IOptions<MaterialConfigSettings> materialConfig,
            IFileEncryptionService fileEncryptionService)
        {
            _materialAuditReportService = materialAuditReport;
            _materialAuditService = materialAuditService;
            _materialConfig = materialConfig.Value;
            _fileEncryptionService = fileEncryptionService;
        }

        [HttpGet("GetMaterialAuditReportByUser")]
        public async Task<ActionResult<IEnumerable<MaterialAuditDto>>> GetMaterialAuditsByUser(string userId)
      {
            var audits = await _materialAuditService.GetMaterialAuditsByUserAsync(userId);
            return Ok(audits);
        }

        [HttpGet("material-audits/date-range")]
        public async Task<ActionResult<IEnumerable<MaterialAuditDto>>> GetMaterialAuditsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var audits = await _materialAuditService.GetMaterialAuditsByDateRangeAsync(startDate, endDate);
            return Ok(audits);
        }
        [HttpGet("GetAuditEntryReport")]
        public async Task<IActionResult> GetAuditEntryReport(string filter)
        {
            var auditFilterDto = JsonConvert.DeserializeObject<MaterialFilterDto>(filter);
            var response = await _materialAuditService.GetMaterialAuditsByFilterAsync(auditFilterDto);
            return Ok(response);
        }
        [HttpGet("GetMaterialAuditReport")]
        public async Task<IActionResult> GetMaterialAuditReport(string filter)
        {
            var materialFilterDto = JsonConvert.DeserializeObject<MaterialFilterDto>(filter);
            var response = await _materialAuditReportService.GetMaterialsAuditWithFilterAsync(materialFilterDto);
        
            return Ok(response);
        }

        [HttpGet("GenerateExcel")]
        public async Task<IActionResult> Download(string parameters)
        {
            var basePath = _materialConfig.DocumentBasePath + "Reports\\";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string reportFilePath = await _materialAuditReportService.GenerateMaterialAuditReport(basePath, parameters);

            var decryptedFilePath = Path.Combine(basePath, Path.GetFileName(reportFilePath));
            Byte[] bytes = await System.IO.File.ReadAllBytesAsync(decryptedFilePath);

            return File(bytes, "application/octet-stream", "UserAuditReport.xlsx");
        }

        [HttpPost("PrintData")]
        public async Task<IActionResult> PrintData([FromBody] PrintParametersDto printParams)
        {
            try
            {
                if (string.IsNullOrEmpty(printParams.Data))
                {
                    return BadRequest("No data provided for printing");
                }

                // Create the reports directory if it doesn't exist
                var basePath = Path.Combine(_materialConfig.DocumentBasePath, "Reports");
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                // Generate a unique filename
                string fileName = $"Report_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(basePath, fileName);

                // Create PDF document with specified orientation
                using var fs = new FileStream(filePath, FileMode.Create);
                using var document = new Document(PageSize.A4, 20, 20, 20, 20);
                
                if (printParams.Orientation?.ToLower() == "landscape")
                {
                    document.SetPageSize(PageSize.A4.Rotate());
                }

                var writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Add title if provided
                if (!string.IsNullOrEmpty(printParams.ReportTitle))
                {
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var title = new Paragraph(printParams.ReportTitle, titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20f
                    };
                    document.Add(title);
                }

                // Create table
                var table = new PdfPTable(printParams.Headers?.Length ?? 1)
                {
                    WidthPercentage = 100
                };

                // Add headers
                if (printParams.Headers != null)
                {
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    foreach (var header in printParams.Headers)
                    {
                        var cell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = new BaseColor(240, 240, 240),
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        };
                        table.AddCell(cell);
                    }
                }

                // Add data rows
                var data = JsonConvert.DeserializeObject<List<string[]>>(printParams.Data);
                var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                
                foreach (var row in data)
                {
                    foreach (var cellContent in row)
                    {
                        var cell = new PdfPCell(new Phrase(cellContent, cellFont))
                        {
                            Padding = 5
                        };
                        table.AddCell(cell);
                    }
                }

                document.Add(table);
                document.Close();

                // Read the generated PDF
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                
                // Clean up the file
                System.IO.File.Delete(filePath);

                // Return the PDF
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }
    }
}
