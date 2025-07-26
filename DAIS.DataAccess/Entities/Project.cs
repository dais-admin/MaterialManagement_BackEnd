namespace DAIS.DataAccess.Entities
{
    public class Project:BaseEntity
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Remarks {  get; set; }
        public Guid? AgencyId { get; set; }
        public virtual Agency Agency { get; set; }
    }
}
