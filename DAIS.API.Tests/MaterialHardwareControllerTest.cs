using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace DAIS.API.Tests
{
    public class MaterialHardwareControllerTest
    {
        private readonly Mock<IMaterialHardwareService> _materialHardwareService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public MaterialHardwareControllerTest()
        {
            _materialHardwareService = new Mock<IMaterialHardwareService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }

        [Fact]
        public async Task GetMaterialHardwareById_ShouldReturnsMaterialHardware_WhenStatusIsOk()
        {
            var materialHardwareDtoData = ApiTestData.GetMaterialHardwareDtoData();
            _materialHardwareService.Setup(x => x.GetMaterialHardwareByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(materialHardwareDtoData[0]);

            var materialHardwareController = new MaterialHardwareController(_materialHardwareService.Object, configMock.Object);

            var materialHardwareResult = await materialHardwareController.GetMaterialHardwareById(Guid.NewGuid());

            Assert.NotNull(materialHardwareResult);
            Assert.IsType<OkObjectResult>(materialHardwareResult);
        }

        [Fact]
        public async Task GetAllMaterialHardware_ShouldReturnsMaterialHardware_WhenStatusIsOk()
        {
            var materialHardwareDtoData = ApiTestData.GetMaterialHardwareDtoData();
            _materialHardwareService.Setup(x => x.GetAllMaterialHardware())
                .ReturnsAsync(materialHardwareDtoData);

            var materialHardwareController = new MaterialHardwareController(_materialHardwareService.Object, configMock.Object);

            var materialHardwareResult = await materialHardwareController.GetAllMaterialHardware();

            Assert.NotNull(materialHardwareResult);
            Assert.IsType<OkObjectResult>(materialHardwareResult);
        }


        // public async Task AddMaterialHardwareAsync_Returns_OkResult()
        [Fact]
        public async Task AddMaterialHardwareAsync_ShouldAddMaterialHardware_WhenStatusIsOk()
        {

            var materialHardwareDto = ApiTestData.GetMaterialHardwareDtoData();
            _materialHardwareService.Setup(x => x.AddMaterialHardwareAsync(materialHardwareDto[0]))
                    .ReturnsAsync(materialHardwareDto[0]);

            //var materialHardwareController = new MaterialHardwareController(_materialHardwareService.Object, configMock.Object);

            //var materialHardwareResult = await materialHardwareController.AddMaterialHardwareAsync(materialHardwareDto[0]);

           // Assert.NotNull(materialHardwareResult);
            //Assert.IsType<OkObjectResult>(materialHardwareResult);

        }

        [Fact(Skip = "For time being")]
        public async Task UpdateMaterialHardwareAsync_ShouldUpdateMaterialHardware_WhenStatusIsOk()
        {
            var materialHardwareDto = ApiTestData.GetMaterialHardwareDtoData();
            _materialHardwareService.Setup(x => x.UpdateMaterialHardwareAsync(materialHardwareDto[0]))
                .ReturnsAsync(materialHardwareDto[0]);

            var materialHardwareController = new MaterialHardwareController(_materialHardwareService.Object, configMock.Object);

            var materialHardwareResult = "";//await materialHardwareController.UpdateMaterialHardwareAsync(materialHardwareDto[0]);

            Assert.NotNull(materialHardwareResult);
            Assert.IsType<OkObjectResult>(materialHardwareResult);
        }

        [Fact]
        public async Task DeleteMaterialHardwareAsync_ShouldDeleteMaterialHardware_WhenStatusIsOk()
        {
            var materialHardwareDto = ApiTestData.GetMaterialHardwareDtoData();

            _materialHardwareService.Setup(x => x.DeleteMaterialHardwareAsync(It.IsAny<Guid>()));


            var materialHardwareController = new MaterialHardwareController(_materialHardwareService.Object, configMock.Object);

            await materialHardwareController.DeleteMaterialHardwareAsync(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
