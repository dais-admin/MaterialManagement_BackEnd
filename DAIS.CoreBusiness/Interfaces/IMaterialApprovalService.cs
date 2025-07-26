using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialApprovalService
    {
        Task<MaterialApprovalDto> GetMaterialApproval(Guid id);
        Task<MaterialApprovalDto> AddMaterialApproval(ApprovalInformationDto approvalInformationDto);
        
        Task<MaterialApprovalDto> UpdateMaterialApproval(MaterialApprovalDto materialApprovalDto);
        Task DeleteMaterialApproval(Guid id);
        Task<List<MaterialApprovalDto>> GetAllMaterialApprovals(Guid workPackageId);
        Task<MaterialApprovalDto> UpdateMaterialApprovalStatus(ApprovalInformationDto approvalInformationDto);
        Task<List<MaterialApprovalDto>> GetMaterialsByStatusAsync(ApprovalStatus approvalStatus, bool isActive, string userId);
        Task<List<MaterialApprovalDto>> GetMaterialListByUserIdAsync(string userId, string userRole);
        Task<MaterialApprovalDto> GetMaterialApproveByMaterialId(Guid materialId);
        Task<MaterialApprovalDto> AddMaterialBulkApproval(BulkApprovalInformationDto bulkApprovalInformationDto);
        Task<BulkApprovalResponseDto> UpdateMaterialBulkApprovalStatus(BulkApprovalInformationDto bulkApprovalInformationDto);
        Task<List<BulkApprovalResponseDto>> GetBulkApprovalMaterialListByUserId(string userId, string userRole);

    }
}
