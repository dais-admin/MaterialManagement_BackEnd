namespace DAIS.DataAccess.Entities
{
    public class SubDivision:BaseEntity
    {
        public string SubDivisionName { get; set; }
        public string SubDivisionCode { get; set; }
        public Guid? DivisionId { get; set; }
        public string? Remarks { get; set; }
        public virtual Division Division { get; set; }
    }
}
