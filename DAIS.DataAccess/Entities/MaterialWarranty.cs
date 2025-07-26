
namespace DAIS.DataAccess.Entities
{
    public class MaterialWarranty:BaseEntity
    {
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public DateTime? LastRenewalDate { get; set; }
        public bool IsExtended {  get; set; }
        public int NoOfMonths {  get; set; }
        public string? WarrantyDocument {  get; set; }
        public Guid ManufacturerId {  get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }       
        public Guid MaterialId {  get; set; }
        public virtual Material Material { get; set; }
       
    }
}
