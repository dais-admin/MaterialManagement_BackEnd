namespace DAIS.CoreBusiness.Dtos
{
    public  class SupplierDto
    {
        public Guid Id {  get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string ProductsDetails { get; set; }
        public string Remarks { get; set; }
        public string? ContactNo { get; set; }
        public string? ContactEmail { get; set; }
        public string? SupplierDocument { get; set; }
        public Guid MaterialTypeId { get; set; }
        public Guid CategoryId { get; set; }
        public MaterialTypeDto? MaterialType { get; set; }
        public CategoryDto? Category {  get; set; }   
    }
}
