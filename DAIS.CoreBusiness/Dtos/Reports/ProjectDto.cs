using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Remarks { get; set; }
        public Guid? AgencyId { get; set; }
        public AgencyDto? Agency { get; set; }
    }
}
