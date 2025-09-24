using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.EntityFrameworkCore;

namespace DAIS.CoreBusiness.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IJwtTokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly MailSettings _mail_Settings = null;
        private readonly IEmailService _mailService;

        

        public AccountService(IGenericRepository<User> genericRepository,
            IJwtTokenService tokenService,
            IMapper mapper,
            ILogger<AccountService> logger,
            UserManager<User> userManager,
            IOptions<MailSettings> mailSettings,
            IEmailService mailService
            
            )
        {
            _genericRepository = genericRepository;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
            _mail_Settings = mailSettings.Value;
           
        }
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation("AccountService:Login:Method Start");
            UserDto userDto = new();
            User existinguser = null;
            try
            {
                existinguser = await GetUserByEmail(loginDto.UserEmail);
                if (existinguser == null)
                {
                    userDto.Email = loginDto.UserEmail;
                    userDto.IsSuccess = false;
                    userDto.Message = "User does not exists";
                }
                else
                {
                    if (await IsValidUser(existinguser, loginDto.Password))
                    {
                        var userRoles= await _userManager.GetRolesAsync(existinguser).ConfigureAwait(false);
                        userDto.Id=Guid.Parse(existinguser.Id);
                        userDto.Email = loginDto.UserEmail;
                        userDto.IsSuccess = true;
                        userDto.UserToken = _tokenService.GenarateToken(existinguser).Result;
                        userDto.UserType = string.Join(",", userRoles);
                        userDto.UserName = existinguser.UserName;
                        userDto.EmployeeName = existinguser.EmployeeName;
                        if (existinguser.SubDivisionId!=null)
                        {
                            userDto.SubDivisionId = existinguser.SubDivisionId;
                            
                        }                      
                        if(existinguser.LocationId!=null)
                        {
                            userDto.LocationId = existinguser.LocationId;
                        }
                        if (existinguser.DivisionId != null)
                        {
                            userDto.DivisionId = existinguser.DivisionId;
                        }
                        userDto.Message = "User Login SuccessFully";

                    }
                    else
                    {
                        userDto.Email = loginDto.UserEmail;
                        userDto.IsSuccess = false;
                        userDto.Message = "InCorrect Password";
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("AccountService:Login:Method End");
            return userDto;
        }
       

        public async Task<UserDto> Register(RegistrationDto registrationDto)
        {
            UserDto userDto = new UserDto();
            _logger.LogInformation("AccountService:Register:Method Start");
            try
            {
                var user = await GetUserByEmail(registrationDto.Email);
                if (user != null)
                {
                    userDto.Email = registrationDto.Email;
                    userDto.IsSuccess = false;
                    userDto.Message = "User already exists";
                }
                else
                {
                    userDto = await AddUser(registrationDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AccountService:Register:Method End");
            return userDto;
        }
       
        private async Task<User> GetUserByEmail(string userEmail)
        {
            var user = await _genericRepository.Query()                
                .FirstOrDefaultAsync(x => x.Email.Trim() == userEmail.Trim());
            if (user is null)
            {
                _logger.LogError($"User does not exist with UserName:{userEmail}");
            }
            return user;
        }

       
        public async Task<bool> SendEmail(MailData mailData)
        {
          return await _mailService.SendEmailAsync(mailData).ConfigureAwait(false);
        }
        public async Task<bool> ForgotPassword(string userEmail)
        {
            bool isSucess = false;
            _logger.LogInformation("AccountService:ForgotPassword:Method Start");
            try
            {
                var user=await GetUserByEmail(userEmail);
                string newPassword = GeneratePassword();
                
                await _userManager.ChangePasswordAsync(user,user.PasswordHash,newPassword);
                if(user!=null)
                {
                    MailData mailData = new MailData()
                    {
                        RecipientEmail = user.Email,
                        RecipientName = user.UserName,
                        EmailSubject = "One Time Password",
                        EmailBody = "Hello " + user.UserName + "\nYour One Time Password: " + newPassword
                    };
                    isSucess= await _mailService.SendEmailAsync(mailData, null);
                   
                }
            }
            catch(Exception ex)
            {

            }
            _logger.LogInformation("AccountService:ForgotPassword:Method End");
            return isSucess;
        }
        private async Task<UserDto> AddUser(RegistrationDto registrationDto)
        {
            UserDto userDto = new UserDto();
            try
            {
                var user = await _userManager.FindByEmailAsync(registrationDto.Email).ConfigureAwait(false);
                if (user is null)
                {
                    user = new User
                    {
                        UserName = registrationDto.UserName,
                        EmployeeName=registrationDto.EmployeeName,
                        NormalizedUserName = registrationDto.UserName,
                        LocationId = registrationDto.LocationId,
                        RegionId = registrationDto.RegionId,
                        DivisionId= registrationDto.DivisionId,
                        SubDivisionId =registrationDto.SubDivisionId,
                        ProjectId=registrationDto.ProjectId,
                        UserPhoto=registrationDto.UserPhoto,
                        UserType = UserTypes.Viewer, //(UserTypes)Enum.Parse(typeof(UserTypes), registrationDto.UserType, true),
                        Email = registrationDto.Email,
                        NormalizedEmail = registrationDto.Email,
                        EmailConfirmed = true,
                        IsInitialLogin = true
                    };
                    var result = await _userManager.CreateAsync(user, registrationDto.Password);
                    if (result.Succeeded)
                    {
                        MailData mailData = new MailData()
                        {
                            RecipientEmail = registrationDto.Email,
                            RecipientName = registrationDto.Email,
                            EmailSubject = "One Time Password",
                            EmailBody = "Hello " + registrationDto.UserName + "\nYour One Time Password: " + registrationDto.Password
                        };
                        _mailService.SendEmailAsync(mailData, null);
                        userDto = new UserDto()
                        {
                            UserName = registrationDto.UserName,
                            UserType = registrationDto.UserType,
                            Message = "User Registered Successfuly",
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
                        Message = "User Email already Registered",
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
       
        private async Task<bool> IsValidUser(User user, string password)
        {
           
            bool isValid= await _userManager.CheckPasswordAsync(user, password);
            return isValid;
        }

        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$!";
            var random = new Random();
            return new string(Enumerable.Repeat(chars,8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            
                bool isSucess = false;
                _logger.LogInformation("AccountService:ChangePassword:Method Start");
                try
                {
                    var user = await GetUserByEmail(changePasswordDto.Email);


                await _userManager.ChangePasswordAsync(user, user.PasswordHash, changePasswordDto.NewPassword);
                    
                }
                catch (Exception ex)
                {

                }
                _logger.LogInformation("AccountService:ChangePassword:Method End");
                return true;
            }
        }
    }

