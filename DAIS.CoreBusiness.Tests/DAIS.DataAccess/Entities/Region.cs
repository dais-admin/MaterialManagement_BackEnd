
namespace DAIS.DataAccess.Entities
{
    public class Region:BaseEntity
    {  
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public string? Remarks { get; set; }
        public virtual ICollection<Material> Assets { get; set; }
    }
}
