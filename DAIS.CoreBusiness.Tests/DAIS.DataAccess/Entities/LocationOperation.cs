
namespace DAIS.DataAccess.Entities
{
    public class LocationOperation:BaseEntity
    {
        
        public string LocationOperationName { get; set; }
        public string LocationOperationCode { get; set; }
        public string System {  get; set; }
        public double? Latitude {  get; set; }
        public double? Longitude { get; set; }
        public string? Remarks { get; set; }
        public Guid WorkPackageId { get; set; }
        public Guid SubDivisionId { get; set; }
        public virtual SubDivision SubDivision { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }
        public virtual ICollection<Material> Assets { get; set; }
    }
}
