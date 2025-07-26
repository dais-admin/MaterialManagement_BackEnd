using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly MaterialConfigSettings _materialConfig;
        public AccountController(IAccountService accountService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _accountService = accountService;
            _materialConfig = materialConfig.Value;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var userDto = await _accountService.Login(loginDto);
            return Ok(userDto);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromForm] IFormFile? userImage, [FromForm] string userData)
        {
            if (userData == null || string.IsNullOrEmpty(userData))
            {
                return BadRequest();
            }
            var registrationDto = JsonConvert.DeserializeObject<RegistrationDto>(userData);
            if (registrationDto == null)
            {
                return BadRequest();
            }

            if (userImage != null)
            {
                registrationDto.UserPhoto = await SaveUserImage(userImage);
            }
            var userDto = await _accountService.Register(registrationDto);
            return Ok(userDto);
        }
        


        [HttpGet]
        public async Task<ActionResult> TestEmail()
        {
            MailData mailData = new MailData()
            {
                EmailBody = "This is Test email",
                EmailSubject = "Test",
                RecipientEmail = "vijay22mali@gmail.com",
                RecipientName = "Vijay Mali"
            };
            bool result = false;//await _accountService.SendEmail(mailData);
            if (!result)
                return BadRequest();
            return Ok(result);

        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult>ChangePassword(ChangePasswordDto changePasswordDto)
        {
            return Ok(await _accountService.ChangePassword(changePasswordDto));
          
        }


    

    [HttpGet("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(string userEmail)
        {
            bool result = await _accountService.ForgotPassword(userEmail);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
        private async Task<string> SaveUserImage([FromForm] IFormFile? userImage)
        {
            var folderName = "MaterialDocument" + "\\" + "UserPhotos" + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = userImage.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await userImage.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }
    }
}
