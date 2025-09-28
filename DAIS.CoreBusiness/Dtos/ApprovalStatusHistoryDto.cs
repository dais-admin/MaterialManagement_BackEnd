
namespace DAIS.CoreBusiness.Dtos
{
    public class ApprovalStatusHistoryDto
    {
        public DateTime? StatusChangeDate { get; set; }
        public string? StatusChangeBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string? Comments { get; set; }
        public string[]? ReviewerApproverEmailIds { get; set; }
        public Guid MaterialId { get; set; }
    }
}
