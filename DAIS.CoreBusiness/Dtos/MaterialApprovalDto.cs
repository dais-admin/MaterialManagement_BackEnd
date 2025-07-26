using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialApprovalDto
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? SubmitterId { get; set; }
        public string? ReveiwerId { get; set; }
        public UserDto? Reveiwer { get; set; }
        public string? ApproverId { get; set; }
        public UserDto? Approver { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? SubmitterComment { get; set; }
        public string? ReviewerPreviousComment { get; set; }
        public string? ReviewerComment { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApproverComment { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid MaterialId { get; set; }
        public MaterialDto? Material { get; set; }
        public bool? IsActive { get; set; }
    }
}
