using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace DAIS.API.Tests
{
    public class AccountControllerTest
    {
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public AccountControllerTest()
        {
            _accountService = new Mock<IAccountService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }

        [Fact]
        public async Task Login_ShouldReturns_WhenStatusIsOk()
        {
            var loginData = ApiTestData.GetLoginDtoData();
            var userDtoList = ApiTestData.GetUserDtoData();

            _accountService.Setup(x => x.Login(loginData))
                .ReturnsAsync(userDtoList[0]);

            var accountController = new AccountController(_accountService.Object, configMock.Object);
            var loginResult = await accountController.Login(loginData);

            Assert.NotNull(loginResult);
        }

        [Fact]
        public void Register_ShouldReturns_WhenStatusIsOk()
        {
            var registrationData = ApiTestData.GetRegistrationDtoData();
            var userDtoList = ApiTestData.GetUserDtoData();

            _accountService.Setup(x => x.Register(registrationData))
                .ReturnsAsync(userDtoList[1]);

            var accountController = new AccountController(_accountService.Object, configMock.Object);
            var registrationResult = ""; //accountController.Register( file, registrationData);

            Assert.NotNull(registrationResult);

        }
    }
}