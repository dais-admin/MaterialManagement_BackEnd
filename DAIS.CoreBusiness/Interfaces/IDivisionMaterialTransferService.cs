﻿using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDivisionMaterialTransferService
    {
        Task<MaterialIssueReceiveResponseDto> AddDivisionMaterialTransfer(DivisionMaterialTransferDto divisionMaterialTransferDto);
        Task<DivisionMaterialTransferDto> GetDivisionMaterialTransferByVoucherNo(string voucherNo);
        Task<List<DivisionMaterialIssueReceiveItemDto>> GetDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId);
        Task<bool> AddDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto);
        Task<List<DivisionMaterialTransferApprovalDto>> GetDivisionMaterialTransfersByIssuingDivision(Guid divisionId);
        Task<bool> UpdateDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
    }
}
