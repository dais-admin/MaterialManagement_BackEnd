
namespace DAIS.DataAccess.Entities
{
    public class MaterialSoftware:BaseEntity
    {
        public string SoftwareName {  get; set; }
        public string? SoftwareDescription { get; set;}
        public int Quantity {  get; set;}
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set; }
        public string? SoftwareDocument {  get; set; }
        public string? Remarks { get; set; }
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material? Material { get; set; }
        
    }
}
