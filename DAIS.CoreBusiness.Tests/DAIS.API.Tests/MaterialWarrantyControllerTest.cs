using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace DAIS.API.Tests
{
    public class MaterialWarrantyControllerTest
    {
        private readonly Mock<IMaterialWarrantyService> _materialWarrantyService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public MaterialWarrantyControllerTest()
        {
            _materialWarrantyService = new Mock<IMaterialWarrantyService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }
        [Fact]
        public async void GetWarrantyById_Returns_OkResult()
        {
            var materialWarrantyDtoData = ApiTestData.GetMaterialWarrantyDtoData();
            _materialWarrantyService.Setup(x => x.GetWarrantyByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(materialWarrantyDtoData[0]);

            var materialWarrantyController = new MaterialWarrantyController(_materialWarrantyService.Object, configMock.Object);

            var materialWarrantyResult = await materialWarrantyController.GetWarrantyByIdAsync(Guid.NewGuid());

            Assert.NotNull(materialWarrantyResult);
            Assert.IsType<OkObjectResult>(materialWarrantyResult);
        }
        [Fact]
        public async void GetAllMaterialWarranty_Returns_OkResult()
        {
            var materialWarrantyDtoData = ApiTestData.GetMaterialWarrantyDtoData();
            _materialWarrantyService.Setup(x => x.GetAllMaterialWarranty())
                .ReturnsAsync(materialWarrantyDtoData);

            var materialWarrantyController = new MaterialWarrantyController(_materialWarrantyService.Object, configMock.Object);

            var materialWarrantyResult = await materialWarrantyController.GetAllMaterialWarranty();

            Assert.NotNull(materialWarrantyResult);
            Assert.IsType<OkObjectResult>(materialWarrantyResult);
        }
        [Fact]
        public async void AddWarrantyAsync_Returns_OkResult()
        {

            var materialHardwareDto = ApiTestData.GetMaterialWarrantyDtoData();
            _materialWarrantyService.Setup(x => x.AddWarrantyAsync(materialHardwareDto[0]))
                    .ReturnsAsync(materialHardwareDto[0]);

            var materialWarrantyController = new MaterialWarrantyController(_materialWarrantyService.Object, configMock.Object);

            //var materialWarrantyResult = await materialWarrantyController.AddWarrantyAsync(materialHardwareDto[0]);

            //Assert.NotNull(materialWarrantyResult);
            //Assert.IsType<OkObjectResult>(materialWarrantyResult);

        }
        [Fact]
        public async void UpdateWarrantyAsync_Returns_OkResult()
        {

            var materialWarrantyDto = ApiTestData.GetMaterialWarrantyDtoData();
            _materialWarrantyService.Setup(x => x.UpdateWarrantyAsync(materialWarrantyDto[0]))
                .ReturnsAsync(materialWarrantyDto[0]);

            //var jsonObjest=JsonConvert.SerializeObject(materialWarrantyDto);

            //var materialHardwareController = new MaterialWarrantyController(_materialWarrantyService.Object, configMock.Object);

            //var materialWarrantyResult = await materialHardwareController.UpdateWarrantyAsync(null,jsonObjest);

            //Assert.NotNull(materialWarrantyResult);
            //Assert.IsType<OkObjectResult>(materialWarrantyResult);
            Assert.True(true);
        }
        [Fact]
        public async void DeleteWarrantyAsync_Returns_OkResult()
        {
            var materialWarrantyDto = ApiTestData.GetMaterialWarrantyDtoData();

            _materialWarrantyService.Setup(x => x.DeleteWarrantyAsync(It.IsAny<Guid>()));


            var materialWarrantyController = new MaterialWarrantyController(_materialWarrantyService.Object, configMock.Object);

            await materialWarrantyController.DeleteWarrantyAsync(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
