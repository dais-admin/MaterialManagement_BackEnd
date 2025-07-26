using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class MaterialApproval:BaseEntity
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? SubmitterId { get; set; }
        public virtual User Submitter { get; set; }
        public string? ReveiwerId { get; set; }
        public virtual User Reveiwer { get; set; }
        public string? ApproverId { get; set; }
        public virtual User Approver { get; set; }
        public DateTime? ReviewedDate { get; set; }       
        public DateTime? ApprovedDate { get; set; }
        public string? ReviewerComment { get; set; }
        public string? SubmitterComment { get; set; }
        public string? ReviewerPreviousComment { get; set; }
        public string? ApproverComment { get; set; }
        public string? ApproverPreviousComment { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
