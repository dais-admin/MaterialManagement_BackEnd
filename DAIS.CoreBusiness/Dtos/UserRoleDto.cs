namespace DAIS.CoreBusiness.Dtos
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? EmployeeName {  get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public IList<string>? UserRoles { get; set; }

    }
}
