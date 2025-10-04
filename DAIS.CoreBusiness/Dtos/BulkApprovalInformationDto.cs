using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class BulkApprovalInformationDto
    {
        public string? ApprovalStatus { get; set; }
        public string? CurrentUserEmailId { get; set; }
        public string?[] ReviewerApproverEmailIds { get; set; }
        public Guid BulkUploadDetailId { get; set; }
        public string? Comment { get; set; }
        public string? BulkUploadFileName { get; set; }
    }
}
