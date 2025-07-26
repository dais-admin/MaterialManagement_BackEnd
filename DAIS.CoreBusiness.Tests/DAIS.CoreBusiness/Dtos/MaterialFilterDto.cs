
namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialFilterDto
    {
        public Guid? ProjectId { get; set; }
        public Guid WorkPackageId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? MaterialId { get; set; }
        public string? System { get; set; }
        public string? KindOfMaterial { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Action { get; set; }
        public string? TableName {  get; set; }
        public string?  UserName { get; set; }
    }
}
