
using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> UpdateUser(RegistrationDto registrationDto);
        Task<List<UserDto>> GetUsersByRole(string roleName);
        Task<UserDto> GetUserByEmailAsync(string userEmail);
        Task<UserDto> GetUserById(Guid userId);
        Task<List<UserDto>> GetAllUsers();
        Task<UserRoleDto> AssignUserRoleAsync(UserRoleDto userRoleDto);
        Task<List<UserRoleDto>> GetAllUsersRole();
        Task DeleteUserById(string userId);
        Task DeleteUserRoleByName(string roleName);
    }
}
