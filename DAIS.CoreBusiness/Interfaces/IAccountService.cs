using DAIS.CoreBusiness.Dtos;
using DAIS.DataAccess.Entities;
using DAIS.Infrastructure.EmailProvider;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IAccountService
    {
       
        Task<UserDto> Register(RegistrationDto registrationDto);
        Task<UserDto> Login(LoginDto loginDto);
        Task<bool> SendEmail(MailData mailData);  
        Task<bool> ForgotPassword(string userEmail);
        Task<bool>ChangePassword(ChangePasswordDto changePasswordDto);
        
    }
}
