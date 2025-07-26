
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public LocationOperationDto Location {  get; set; }
        public RegionDto Region { get; set; }
        
        public ProjectDto Project { get; set; }
        public  SubDivisionDto SubDivision { get; set; }
        public string UserToken { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }

    public class UserRoleFeature
    {
        public string FeatureName { get; set; }

        public string PermissionName { get; set; }

        public string RoleFeatureName => FeatureName + ":" + PermissionName;
    }

}
