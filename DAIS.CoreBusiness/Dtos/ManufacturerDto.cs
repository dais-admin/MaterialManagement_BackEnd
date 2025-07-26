
namespace DAIS.CoreBusiness.Dtos
{
    public  class ManufacturerDto
    {
        public Guid Id { get; set; } 
        public string ManufacturerName { get; set; }
        public string ManufacturerAddress { get; set; }
        public string ProductsDetails { get; set; }
        public string ImportantDetails { get; set; }
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? ManufacturerDocument { get; set; }
        public Guid? MaterialTypeId { get; set; }
        public MaterialTypeDto? MaterialType { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual CategoryDto? Category { get; set; }


    }
}
