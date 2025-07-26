using DAIS.API.Controllers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace DAIS.API.Tests
{
    public class LocationOperationControllerTest
    {
        private readonly Mock<ILocationOperationService> _locationOperationService;
        public LocationOperationControllerTest() 
        {
            _locationOperationService = new Mock<ILocationOperationService>();
        }
        [Fact]
        public async Task GetLocationOperation_ShouldReturnLocationOfOperation_WhenStatusIsOk()
        {
            var locationOperationDtoData = ApiTestData.GetLocationOperationDtoData();
            _locationOperationService.Setup(x => x.GetLocationOperation(It.IsAny<Guid>()))
                .ReturnsAsync(locationOperationDtoData[0]);

            var locationoperationController = new LocationOperationController(_locationOperationService.Object);

            var locationoperationResult = await locationoperationController.GetLocationOperation(Guid.NewGuid());

            Assert.NotNull(locationoperationResult);
            Assert.IsType<OkObjectResult>(locationoperationResult);

        }

        [Fact]
        public async Task GetAllLocationOperation_Returns_OkResult()
        {
            var locationOperationDtoData = ApiTestData.GetLocationOperationDtoData();
            _locationOperationService.Setup(x => x.GetAllLocationOperation())
                .ReturnsAsync(locationOperationDtoData);

            var locationOperationController = new LocationOperationController(_locationOperationService.Object);

            var locationOperationResult = await locationOperationController.GetAllLocationOperation();

            Assert.NotNull(locationOperationResult);
            Assert.IsType<OkObjectResult>(locationOperationResult);
        }

        [Fact]
        public async Task  Add_ShouldAddLocationOfOpration_WhenStatusIsOk()
        {

            var locationOperationDto = ApiTestData.GetLocationOperationDtoData();
            _locationOperationService.Setup(x => x.AddLocationOperation(locationOperationDto[0]))
                    .ReturnsAsync(locationOperationDto[0]);

            var locationOperationController = new LocationOperationController(_locationOperationService.Object);

            var locationOperationResult = await locationOperationController.AddLocationOperation(locationOperationDto[0]);

            Assert.NotNull(locationOperationResult);
            Assert.IsType<OkObjectResult>(locationOperationResult);

        }

        [Fact]
        public async Task Update_ShouldUpdateDivision_WhenStatusIsOk()
        {
            var locationOperationDto = ApiTestData.GetLocationOperationDtoData();
            _locationOperationService.Setup(x => x.UpdateLocationOperation(locationOperationDto[0]))
                .ReturnsAsync(locationOperationDto[0]);

            var locationOperationController = new LocationOperationController(_locationOperationService.Object);

            var locationOperationResult = await locationOperationController.UpdateLocationOperation(locationOperationDto[0]);

            Assert.NotNull(locationOperationResult);
            Assert.IsType<OkObjectResult>(locationOperationResult);
        }

        [Fact]
        public async Task Delete_ShouldLocationOperation_WhenStatusIsOk()
        {
            var locationOperationDto = ApiTestData.GetLocationOperationDtoData();

            _locationOperationService.Setup(x => x.DeleteLocationOperation(It.IsAny<Guid>()));


            var locationOperationController = new LocationOperationController(_locationOperationService.Object);

            await locationOperationController.DeleteLocationOperation(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
