using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Interfaces.Report
{
    public interface IMaterialAuditReportService
    {
        Task<List<MaterialAuditReportDto>> GetMaterialAuditReport();
        Task<string> GenerateMaterialAuditReport(string folderPath, string parameters);
        Task<List<MaterialAuditReportDto>> GetMaterialsAuditWithFilterAsync(MaterialFilterDto materialFilterDto);
    }
}
