using System.ComponentModel.DataAnnotations;

namespace DAIS.DataAccess.Entities
{
    public class BaseEntity
    {
        
        [Key]
        public Guid Id { get; set; } 
        public string CreatedBy {  get; set; }
        public string UpdatedBy {  get; set; }
        public DateTime CreatedDate {  get; set; }
        public DateTime UpdatedDate { get; set; }     
        public bool IsDeleted { get; set; }=false;
        public BaseEntity() 
        { 
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
