namespace DAIS.DataAccess.Entities
{
    public class MaterialType : BaseEntity
    {
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
        public string? Remarks { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
