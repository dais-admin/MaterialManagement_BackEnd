using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IGenericRepository<User> _genericRepository;
        public UserService(IGenericRepository<User> genericRepository,IMapper mapper,
            ILogger<UserService> logger,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _genericRepository = genericRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> UpdateUser(RegistrationDto registrationDto)
        {
            UserDto userDto = new UserDto();
            try
            {
                var user = await _userManager.FindByEmailAsync(registrationDto.Email).ConfigureAwait(false);
                if (user != null)
                {
                    user.EmployeeName = registrationDto.EmployeeName;
                    user.LocationId = registrationDto.LocationId;
                    user.RegionId = registrationDto.RegionId;
                    user.SubDivisionId = registrationDto.SubDivisionId;
                    //ProjectId = registrationDto.ProjectId,
                    user.UserPhoto = registrationDto.UserPhoto;
                    //UserType = UserTypes.Viewer, //(UserTypes)Enum.Parse(typeof(UserTypes), registrationDto.UserType, true),
                    user.Email = registrationDto.Email;
                    user.NormalizedEmail = registrationDto.Email;
                   
                   
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        userDto = new UserDto()
                        {
                            UserName = registrationDto.UserName,
                            UserType = registrationDto.UserType,
                            Message = "User Updated Successfuly",
                            IsSuccess = true

                        };
                    }
                    else
                    {
                        userDto = new UserDto()
                        {
                            UserName = registrationDto.UserName,
                            UserType = registrationDto.UserType,
                            Message = result.Errors.ToString(),
                            IsSuccess = false

                        };
                    }

                }
                else
                {
                    userDto = new UserDto()
                    {
                        UserName = registrationDto.UserName,
                        UserType = registrationDto.UserType,
                        Message = "User not exists with this Email",
                        IsSuccess = false

                    };

                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return userDto;
        }
        public async Task<List<UserDto>> GetUsersByRole(string roleName)
        {
            _logger.LogInformation("UserService:GetUsersByRole:Method Start");
            List<UserDto> userListDto = new List<UserDto>();
            try
            {
                var users=await _userManager.GetUsersInRoleAsync(roleName);
                foreach (var user in users)
                {
                    userListDto.Add(
                        new UserDto()
                        {
                            Id=Guid.Parse(user.Id),
                            EmployeeName=user.EmployeeName,
                            UserName=user.UserName,
                            Email=user.Email,                       
                        }
                   );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("UserService:GetUsersByRole:Method End");
            return userListDto;
        }
        public async Task<UserRoleDto> AssignUserRoleAsync(UserRoleDto userRoleDto)
        {
            _logger.LogInformation("AccountService:AssignUserRoleAsync:Method Start");
            try
            {
                var user = await GetUserByEmail(userRoleDto.UserEmail);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, userRoleDto.RoleName).ConfigureAwait(false);
                    
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AccountService:AssignUserRoleAsync:Method End");
            return userRoleDto;
        }
        public async Task<UserDto> GetUserByEmailAsync(string userEmail)
        {
            _logger.LogInformation("AccountService:GetUserByEmailAsync:Method Start");
            UserDto userDto = new UserDto();
            try
            {
                var user = await GetUserByEmail(userEmail);
                if (user != null)
                {
                    userDto = _mapper.Map<UserDto>(user);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AccountService:GetUserByEmailAsync:Method End");
            return userDto;
        }
        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await _genericRepository.GetById(userId).ConfigureAwait(false);
            if (user is null)
            {
                _logger.LogError($"User does not exist with User Id:{userId}");
            }

            return _mapper.Map<UserDto>(user);
        }
        public async Task DeleteUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogError($"User does not exist with User Id:{userId}");
            }
            await _userManager.DeleteAsync(user);          
        }
        public async Task DeleteUserRoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
            if (role is null)
            {
                _logger.LogError($"User does not exist with User Id:{roleName}");
            }
            await _roleManager.DeleteAsync(role);
        }
        public async Task<List<UserRoleDto>> GetAllUsersRole()
        {
            _logger.LogInformation("AccountService:GetAllUsersRole:Method Start");
            List<UserRoleDto> userRoleDtoList = new List<UserRoleDto>();
            try
            {
                var users = await _genericRepository.Query()
                   .ToListAsync().ConfigureAwait(false);
                foreach (var user in users)
                {
                    var usersRoles = await _userManager.GetRolesAsync(user);
                    var userRoleDto = new UserRoleDto()
                    {
                        UserId = new Guid(user.Id),
                        UserEmail = user.Email,
                        UserRoles = usersRoles,
                        RoleName = usersRoles.FirstOrDefault(),
                        EmployeeName = user.UserName

                    };
                    userRoleDtoList.Add(userRoleDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AccountService:GetAllUsersRole:Method End");
            return userRoleDtoList;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            List<UserDto> userDtoList = new List<UserDto>();
            try
            {
                var users = await _genericRepository.Query()
                    .Include(x => x.Location)
                    .Include(x => x.Region)
                    .Include(x=>x.SubDivision)
                    .Include(x=>x.Project)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    userDto.Location = _mapper.Map<LocationOperationDto>(user.Location);
                    userDto.Region = _mapper.Map<RegionDto>(user.Region);
                    userDto.SubDivision = _mapper.Map<SubDivisionDto>(user.SubDivision);
                    if(user.Project != null)
                    {
                        userDto.Project = _mapper.Map<ProjectDto>(user.Project);
                    }
                    
                    userDtoList.Add(userDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return userDtoList;
        }
        private async Task<User> GetUserByEmail(string userEmail)
        {
            var user = await _genericRepository.Query()
                .Include(x => x.Location)
                .Include(x => x.Region)
                .Include(x => x.SubDivision)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Email.Trim() == userEmail.Trim())
                .ConfigureAwait(false);
            if (user is null)
            {
                _logger.LogError($"User does not exist with UserName:{userEmail}");
            }
            return user;
        }
    }
}
