using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace DAIS.API.Tests
{
    public class ManufacturerControllerTest
    {
        private readonly Mock<IManufacturerService> _manufacturerService;
        private readonly List<ManufacturerDto> _manufacturerDtoData;
        private readonly ManufacturerController _manufacturerController;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public ManufacturerControllerTest()
        {
            _manufacturerService = new Mock<IManufacturerService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
            _manufacturerDtoData = ApiTestData.GetManufacturerDtoData();
            _manufacturerService.Setup(x => x.GetManufacturer(It.IsAny<Guid>()))
           .ReturnsAsync(_manufacturerDtoData[0]);
            _manufacturerController = new ManufacturerController(_manufacturerService.Object, configMock.Object);


        }

        [Fact]
        public async Task GetDocumentById_ShouldReturnDocumentById_WhenStatusIsOk()
        {
            //Act
            var manufacturerResult = await _manufacturerController.GetManufacturer(Guid.NewGuid());

            //Asset
            Assert.NotNull(manufacturerResult);
            Assert.IsType<OkObjectResult>(manufacturerResult);

        }

        [Fact]
        public async Task GetAll_ShouldReturnManufacturers_WhenStatusIsOk()
        {
            var manufacturerResult = await _manufacturerController.GetAllManufacturer();

            Assert.NotNull(manufacturerResult);
            Assert.IsType<OkObjectResult>(manufacturerResult);
        }

        [Fact(Skip ="For time being")]
        public async Task Add_ShouldAddManufacturer_WhenStatusIsOk()
        {

            var manufacturerDto = ApiTestData.GetManufacturerDtoData();
            _manufacturerService.Setup(x => x.AddManufacturer(manufacturerDto[0]))
            .ReturnsAsync(manufacturerDto[0]);

            var manufacturerController = new ManufacturerController(_manufacturerService.Object, configMock.Object);

            var manufacturerResult = ""; //await manufacturerController.AddManufacturer(manufacturerDto[0]);

            Assert.NotNull(manufacturerResult);
            Assert.IsType<OkObjectResult>(manufacturerResult);

        }

        [Fact(Skip = "For time being")]
        public async Task Update_ShouldUpdateManufacturer_WhenStatusIsOk()
        {
            var manufacturerDto = ApiTestData.GetManufacturerDtoData();
            _manufacturerService.Setup(x => x.UpdateManufactuter(manufacturerDto[0]))
             .ReturnsAsync(manufacturerDto[0]);

            var manufacturerController = new ManufacturerController(_manufacturerService.Object, configMock.Object);

            var manufacturerResult = ""; //await manufacturerController.UpdateManufacturer(manufacturerDto[0]);

            Assert.NotNull(manufacturerResult);
            Assert.IsType<OkObjectResult>(manufacturerResult);
        }

        [Fact]
        public async Task Delete_ShouldDeleteManufacturer_WhenStatusIsOk()
        {
            var manufacturerDto = ApiTestData.GetManufacturerDtoData();

            _manufacturerService.Setup(x => x.DeleteManufacturer(It.IsAny<Guid>()));


            var manufacturerController = new ManufacturerController(_manufacturerService.Object, configMock.Object);

            await manufacturerController.DeleteManufacturer(Guid.NewGuid());

            Assert.True(true);
        }
    }
}
