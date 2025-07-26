using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDivisionToSubDivisionMaterialTransferService
    {
        Task<DivisionToSubDivisionMaterialTransferDto> GetDivisionToSubDivisionMaterialTransferByVoucherNo(string voucherNo);
        Task<MaterialIssueReceiveResponseDto> AddDivisionToSubDivisionMaterialTransfer(DivisionToSubDivisionMaterialTransferDto divisionToSubDivisionMaterialTransferDto);
        Task UpdateStock(DivisionToSubDivisionMaterialTransfer divisionToSubDivisionMaterialTransfer);
        Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<DivisionToSubDivisionMaterialTransferItemDto> divisionToSubDivisionMaterialTransferItemDtos);
        Task<bool> AddDivisionToSubDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto);
        Task<bool> UpdateDivisionToSubDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
        Task<List<DivisionToSubDivisionMaterialTransferApprovalDto>> GetDivisionToSubDivisionMaterialTransfersByIssuingDivision(Guid divisionId);
        Task<List<DivisionToSubDivisionMaterialIssueReceiveItemDto>> GetDivisionToSubDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
    }
}
