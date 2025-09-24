using System.ComponentModel.DataAnnotations.Schema;

namespace DAIS.DataAccess.Entities
{
    public class MaterialMaintenance:BaseEntity
    {     
        public DateTime MaintenanceStartDate { get; set; }
        public DateTime MaintenanceEndDate { get; set; }
        public string? AgencyAddress { get; set; }
        public string? MaintenanceDocument { get; set; }

        [ForeignKey("MaterialId")]
        public Guid MaterialId {  get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("AgencyId")]
        public Guid? AgencyId { get; set; }
        public virtual Agency Agency { get; set; }
        
  


    }
}
