
using DAIS.DataAccess.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class ApprovalStatusHistory:BaseEntity
    {
        public DateTime? StatusChangeDate { get; set; }
        public string? StatusChangeBy { get; set; }
        public string ActionRequiredByUserEmail {  get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? Comments { get; set; }
        
        // Navigation properties
        [ForeignKey("MaterialId")]
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
    }
}
