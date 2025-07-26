namespace DAIS.DataAccess.Entities
{
    public class Manufacturer:BaseEntity
    {
        public string ManufacturerName {  get; set; }
        public string? ManufacturerAddress { get; set;}
        public string? ProductsDetails { get; set;}
        public string? ImportantDetails { get; set;}
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? Remarks { get; set; }
        public string? ManufacturerDocument {  get; set; }
        public Guid? MaterialTypeId { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Material> Assets { get; set; }

    }
}
