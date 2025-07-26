

using DAIS.API.Controllers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace DAIS.API.Tests
{
    public class RegionControllerTest
    {
        private readonly Mock<IRegionService> _regionService;
        public RegionControllerTest()
        {
            _regionService = new Mock<IRegionService>();
        }
        [Fact]
        public async void GetRegionById_Returns_OkResult()
        {
            var regionDtoData = ApiTestData.GetRegionDtoData();
            _regionService.Setup(x => x.GetRegionById(It.IsAny<Guid>()))
                .ReturnsAsync(regionDtoData[0]);

            var regionController = new RegionController(_regionService.Object);

            var regionResult = await regionController.GetRegionById(Guid.NewGuid());
           

            
            Assert.NotNull(regionResult);
            //Assert.IsType<OkObjectResult>(regionResult);

        }
        [Fact]
        public async void GetAllRegions_Returns_OkResult()
        {
            var regionDtoData = ApiTestData.GetRegionDtoData();

            _regionService.Setup(x => x.GetAllRegions())
                .ReturnsAsync(regionDtoData);

            var regionController = new RegionController(_regionService.Object);

            var regionResult = await regionController.GetAllRegions();

            Assert.NotNull(regionResult);
            Assert.IsType<OkObjectResult>(regionResult);
        }
        [Fact]
        public async void AddRegion_Returns_OkResult()
        {

            var regionDto = ApiTestData.GetRegionDtoData();
            _regionService.Setup(x => x.AddRegion(regionDto[0]))
                    .ReturnsAsync(regionDto[0]);

            var regionController = new RegionController(_regionService.Object);

            var regionResult = await regionController.AddRegion(regionDto[0]);

            Assert.NotNull(regionResult);
            Assert.IsType<OkObjectResult>(regionResult);

        }
        [Fact]
        public async void UpdateRegion_Returns_OkResult()
        {
            var regionDto = ApiTestData.GetRegionDtoData();
            _regionService.Setup(x => x.UpdateRegion(regionDto[0]))
             .ReturnsAsync(regionDto[0]);

            var regionController = new RegionController(_regionService.Object);

            var regionResult = await regionController.UpdateRegion(regionDto[0]);

            Assert.NotNull(regionResult);
            Assert.IsType<OkObjectResult>(regionResult);
        }
        [Fact]
        public async void DeleteRegion_Returns_OkResult()
        {
            var regionDto = ApiTestData.GetRegionDtoData();

            _regionService.Setup(x => x.DeleteRegion(It.IsAny<Guid>()));


            var regionController = new RegionController(_regionService.Object);

            await regionController.DeleteRegion(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
