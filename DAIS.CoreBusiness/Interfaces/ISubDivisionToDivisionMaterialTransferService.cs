using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface ISubDivisionToDivisionMaterialTransferService
    {
        Task<SubDivisionToDivisionMaterialTransferDto> GetSubDivisionToDivisionMaterialTransferByVoucherNo(string voucherNo);
        Task<MaterialIssueReceiveResponseDto> AddSubDivisionToDivisionMaterialTransfer(SubDivisionToDivisionMaterialTransferDto subDivisionToDivisionMaterialTransferDto);
        Task UpdateStock(SubDivisionToDivisionMaterialTransfer subDivisionToDivisionMaterialTransfer);
        Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<SubDivisionToDivisionMaterialTransferItemDto> subDivisionToDivisionMaterialTransferItemDtos);
        Task<bool> AddSubDivisionToDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto);
        Task<bool> UpdateSubDivisionToDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
        Task<List<SubDivisionToDivisionMaterialTransferApprovalDto>> GetSubDivisionToDivisionMaterialTransfersByIssuingSubDivision(Guid subDivisionId);
        Task<List<SubDivisionToDivisionMaterialIssueReceiveItemDto>> GetSubDivisionToDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
    }
}
