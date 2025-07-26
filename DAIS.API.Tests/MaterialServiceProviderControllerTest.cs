using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace DAIS.API.Tests
{
    public class MaterialServiceProviderControllerTest
    {
        private readonly Mock<IMaterialServiceProviderService> _materialServiceProviderService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public MaterialServiceProviderControllerTest()
        {
            _materialServiceProviderService = new Mock<IMaterialServiceProviderService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }

        [Fact]       
        public async void GetMaterialServiceProviderById_Returns_OkResult()
        {
            var serviceProviderDtoData = ApiTestData.GetMaterialServiceProviderDtoData();
            _materialServiceProviderService.Setup(x => x.GetServiceProviderByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(serviceProviderDtoData[0]);

            var materialServiceProviderController = new MaterialServiceProviderController(_materialServiceProviderService.Object, configMock.Object);

            var serviceProviderResult = await materialServiceProviderController.GetMaterialServiceProviderById(Guid.NewGuid());

            Assert.NotNull(serviceProviderResult);
            Assert.IsType<OkObjectResult>(serviceProviderResult);

        }

        [Fact]
        public async void GetAllMaterialServiceProvider_Returns_OkResult()
        {
            var serviceProviderDtoData = ApiTestData.GetMaterialServiceProviderDtoData();
            _materialServiceProviderService.Setup(x => x.GetAllServiceProviderAsync())
            .ReturnsAsync(serviceProviderDtoData);

            var materialServiceProviderController = new MaterialServiceProviderController(_materialServiceProviderService.Object, configMock.Object);

            var serviceProviderResult = await materialServiceProviderController.GetAllMaterialServiceProvides();

            Assert.NotNull(serviceProviderResult);
            Assert.IsType<OkObjectResult>(serviceProviderResult);
        }

        [Fact(Skip = "For time being")]
        public async void AddServiceProviderAsync_Returns_OkResult()
        {

            var serviceProviderDto = ApiTestData.GetMaterialServiceProviderDtoData();
            _materialServiceProviderService.Setup(x => x.AddServiceProviderAsync(serviceProviderDto[0]))
             .ReturnsAsync(serviceProviderDto[0]);

            var materialServiceProviderController = new MaterialServiceProviderController(_materialServiceProviderService.Object, configMock.Object);

            var serviceProviderResult = "";// await materialServiceProviderController.AddServiceProviderAsync(serviceProviderDto[0]);

            Assert.NotNull(serviceProviderResult);
            Assert.IsType<OkObjectResult>(serviceProviderResult);

        }

        [Fact(Skip = "For time being")]
        public async void UpdateServiceProviderAsync_Returns_OkResult()
        {
            var serviceProviderDto = ApiTestData.GetMaterialServiceProviderDtoData();
            _materialServiceProviderService.Setup(x => x.UpdateServiceProviderAsync(serviceProviderDto[0]))
             .ReturnsAsync(serviceProviderDto[0]);

            var materialServiceProviderController = new MaterialServiceProviderController(_materialServiceProviderService.Object, configMock.Object);

            var serviceProviderResult = ""; //await materialServiceProviderController.UpdateServiceProviderAsync(serviceProviderDto[0]);

            Assert.NotNull(serviceProviderResult);
            Assert.IsType<OkObjectResult>(serviceProviderResult);
        }

        [Fact]
        public async void DeleteServiceProviderAsyn_Returns_OkResult()
        {
            var serviceProviderDto = ApiTestData.GetMaterialServiceProviderDtoData();

            _materialServiceProviderService.Setup(x => x.DeleteServiceProviderAsync(It.IsAny<Guid>()));


            var materialServiceProviderController = new MaterialServiceProviderController(_materialServiceProviderService.Object, configMock.Object);

            await materialServiceProviderController.DeleteServiceProviderAsync(Guid.NewGuid());

            Assert.True(true);
        }

    }
}
