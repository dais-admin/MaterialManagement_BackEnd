using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IApprovalStatusHistoryService
    {
        Task<bool> AddApprovalStatusHistory(ApprovalStatusHistoryDto approvalStatusHistoryDto);
        Task<IEnumerable<MaterialDto>> GetMaterialsWithStatusHistoryByUser(List<string>? approvalStatuses, string currentUserEmail);
    }
}
