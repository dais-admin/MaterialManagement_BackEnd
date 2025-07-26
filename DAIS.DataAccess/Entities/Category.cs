
namespace DAIS.DataAccess.Entities
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string? Remarks { get; set; }

        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public Guid? MaterialTypeId { get; set; }
        public virtual MaterialType MaterialType { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
