
namespace DAIS.DataAccess.Entities
{
    public class Supplier:BaseEntity
    {
        public string SupplierName {  get; set; }
        public string? SupplierAddress { get; set;}
        public string? ProductsDetails { get; set;}
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? Remarks { get; set;}
        public string? SupplierDocument {  get; set; }
        public Guid? MaterialTypeId { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Material> Materials { get; set; }

    }
}
