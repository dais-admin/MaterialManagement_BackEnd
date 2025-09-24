using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class RegistrationDto
    {      
        public string UserName { get; set; }
        public string EmployeeName {  get; set; }
        public string Password { get; set; }
        public string? UserType { get; set; }
        public string Email { get; set; }
        public Guid RegionId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? SubDivisionId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid ProjectId { get; set; }      
        public string? UserPhoto {  get; set; }

    }
}
