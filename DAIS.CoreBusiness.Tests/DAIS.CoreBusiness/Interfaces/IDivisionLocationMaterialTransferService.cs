using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDivisionLocationMaterialTransferService
    {
        Task<MaterialIssueReceiveResponseDto> AddDivisionLocationMaterialTransfer(DivisionLocationMaterialTransferDto divisionLocationMaterialTransferDto);
        Task<DivisionLocationMaterialTransferDto> GetDivisionLocationMaterialTransferByVoucherNo(string voucherNo);
        Task<List<DivisionLocationMaterialIssueReceiveItemDto>> GetDivisionLocationMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
        Task<bool> AddDivisionLocationMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto);
        Task<List<DivisionLocationMaterialTransferApprovalDto>> GetDivisionLocationMaterialTransfersByIssuingDivision(Guid divisionId);
        Task<bool> UpdateDivisionLocationMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
    }
}
