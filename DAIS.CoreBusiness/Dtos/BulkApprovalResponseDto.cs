using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class BulkApprovalResponseDto
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public UserDto? Submitter { get; set; }     
        public UserDto? Reveiwer { get; set; }      
        public UserDto? Approver { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? SubmitterComment { get; set; }
        public string? ReviewerComment { get; set; }        
        public DateTime? ApprovedDate { get; set; }
        public string? ApproverComment { get; set; }
        public BulkUploadDetailsDto? BulkUploadDetails { get; set; }
    }
}
