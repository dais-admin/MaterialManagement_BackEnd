
namespace DAIS.DataAccess.Entities
{
    public class MaterialHardware:BaseEntity
    {
        public string HarwareName {  get; set; }
        public int SerialNo {  get; set; }  
        public string? Chipset {  get; set; }
        public  DateTime? DateOfManufacturer { get; set; }
        public string? NetworkDetails {  get; set; } 
        public string? DiskDetails { get; set; }   
        public string? BiosDetails { get; set; }
        public int Quantity { get; set; }
        public string? HardwareDocument {  get; set; }
        public string? Remarks { get; set; }
        public Guid? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public Guid ManufacturerId {  get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
        
    }
}
