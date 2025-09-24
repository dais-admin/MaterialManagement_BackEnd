using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialDto> AddMaterialAsync(MaterialDto assetDto);
        Task<MaterialDto> UpdateMaterialAsync(MaterialDto assetDto);
        Task DeleteMaterialAsync(Guid id);
        Task<MaterialDto> GetMaterialByIdAsync(Guid id);
        Task<List<MaterialDto>> GetAllMaterialsAsync(Guid workPackageId);
        Task<MaterialMasterListDto> GetAllMaterialMasters();
        Task<List<MaterialAuditReportDto>> GetMaterialAuditReport();
        Task<MaterialDto> GetMaterialsByCodeAsync(string materialCode);
        Task<List<MaterialDto>> GetMaterialsBySystemAsync(string systemName, bool isRehabilitation);
        Task<List<MaterialDto>> GetMaterialsWithFilterAsync(MaterialFilterDto materialFilterDto);
        Task DeleteBulkUploadMaterials(Guid bulkUploadDetailId);
        Task<List<Guid>> GetAllMaterialIdsByBulkUploadIdAsync(Guid bulkUploadDetailId);
        Task<List<MaterialDto>> GetMaterialsAddedByCurrentUser();
        Task<List<MaterialDto>> GetAllMaterialsByLocationAsync(Guid locationId);
        Task<List<MaterialDto>> GetAllMaterialsByDivisionAsync(Guid divisionId);
        Task<List<MaterialDto>> GetAllMaterialsBySubDivisionAsync(Guid SubdivisionId);
    }
}
