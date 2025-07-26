
namespace DAIS.DataAccess.Entities
{
    public class Division:BaseEntity
    {
        public string DivisionName {  get; set; }
        public string DivisionCode { get; set;}       
        public Guid? LocationId { get; set; }
        public string? Remarks { get; set; }
        public virtual LocationOperation Location { get; set; }
        public virtual ICollection<Material> Assets { get; set; }
    }
}
