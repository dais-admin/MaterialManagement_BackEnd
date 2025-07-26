namespace DAIS.DataAccess.Entities
{
    public class MaterialMaintenance:BaseEntity
    {     
        public DateTime MaintenanceStartDate { get; set; }
        public DateTime MaintenanceEndDate { get; set; }        
        public Guid MaterialId {  get; set; }
        public virtual Material Material { get; set; }
        
    }
}
