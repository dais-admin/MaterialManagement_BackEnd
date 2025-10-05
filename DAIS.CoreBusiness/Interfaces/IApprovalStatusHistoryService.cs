using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IApprovalStatusHistoryService
    {
        Task<bool> AddApprovalStatusHistory(ApprovalStatusHistoryDto approvalStatusHistoryDto);
        Task<bool> AddBulkApprovalStatusHistory(BulkApprovalInformationDto bulkApprovalInformationDto);
        Task<IEnumerable<MaterialDto>> GetMaterialsWithStatusHistoryByUser(List<string>? approvalStatuses, string currentUserEmail);
        Task<IEnumerable<BulkUploadDetailsDto>> GetBulkApprovalMaterialsWithStatusHistoryByUser(List<string>? approvalStatuses, string currentUserEmail);
        Task<IEnumerable<MaterialDto>> GetMaterialsStatusByProjectWorkpackage(string approvalStatus, Guid workpackageId, Guid? locationId);
    }
}
