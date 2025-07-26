﻿﻿﻿using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialTransferService
    {
        Task<MaterialTransferApprovalResponseDto> AddMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalDto);
        Task<List<LocationMaterialTransferDto>> GetAllMaterialIssueRecieveVoucherByStatus(ApprovalStatus status);
        Task<List<LocationMaterialTransferDto>> GetLocationMaterialTransfersByIssuingLocation(Guid locationId);
        Task<bool> UpdateLocationMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus);
        Task<UserMaterialTransferDto> GetUserMaterialTransfers(string userId);
    }
}
