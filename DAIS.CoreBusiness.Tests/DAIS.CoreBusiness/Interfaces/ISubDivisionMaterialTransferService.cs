using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface ISubDivisionMaterialTransferService
    {
        Task<MaterialIssueReceiveResponseDto> AddSubDivisionMaterialTransfer(SubDivisionMaterialTransferDto subDivisionMaterialTransferDto);
        Task<SubDivisionMaterialTransferDto> GetSubDivisionMaterialTransferByVoucherNo(string voucherNo);
        Task<List<SubDivisionMaterialIssueReceiveItemDto>> GetSubDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
        Task<bool> AddSubDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto);
        Task<List<SubDivisionMaterialTransferApprovalDto>> GetSubDivisionMaterialTransfersByIssuingSubDivision(Guid subDivisionId);
        Task<bool> UpdateSubDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
    }
}
