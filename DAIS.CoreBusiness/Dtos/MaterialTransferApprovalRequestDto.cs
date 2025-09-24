using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialTransferApprovalRequestDto
    {
        public string VoucherNo { get; set; }
        public string? CurrentUserId { get; set; }
        public string ReviewerApproverId { get; set; }
        public string? Comment { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
