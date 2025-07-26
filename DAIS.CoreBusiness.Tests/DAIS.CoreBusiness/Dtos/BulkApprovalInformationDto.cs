using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class BulkApprovalInformationDto
    {
        public ApprovalStatus? ApprovalStatus { get; set; }
        public string? CurrentUserId { get; set; }
        public string?[] ReviewerApproverIds { get; set; }
        public Guid BulkUploadDetailId { get; set; }
        public string? Comment { get; set; }
        public string? BulkUploadFileName { get; set; }
    }
}
