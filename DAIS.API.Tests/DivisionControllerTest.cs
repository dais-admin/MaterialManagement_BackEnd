using DAIS.API.Controllers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.API.Tests
{
    public class DivisionControllerTest
    {
        private readonly Mock<IDivisionService> _divisionService;
        public DivisionControllerTest()
        {
            _divisionService = new Mock<IDivisionService>();

        }

        [Fact]
        public async Task GetDivision_ShouldReturns_WhenStatusIsOk()
        {
            var divisionDtoData = ApiTestData.GetDivisionDtoData();

            _divisionService.Setup(x => x.GetDivision(It.IsAny<Guid>()))
                .ReturnsAsync(divisionDtoData[0]);

            var divisionController = new DivisionController(_divisionService.Object);

            var divisionResult = await divisionController.GetDivision(Guid.NewGuid());

            Assert.NotNull(divisionResult);
            Assert.IsType<OkObjectResult>(divisionResult);

        }

        [Fact]
        public async Task GetAllDivisions_ShouldReturns_WhenStatusIsOk()
        {
            var divisionDtoData = ApiTestData.GetDivisionDtoData();
            _divisionService.Setup(x => x.GetAllDivision())
                .ReturnsAsync(divisionDtoData);

            var divisionController = new DivisionController(_divisionService.Object);

            var divisionResult = await divisionController.GetDivision(Guid.NewGuid());

            Assert.NotNull(divisionResult);
            Assert.IsType<OkObjectResult>(divisionResult);
        }

        [Fact]
        public async Task AddDivision_ShouldReturns_WhenStatusIsOk()
        {

            var divisionDto = ApiTestData.GetDivisionDtoData();
            _divisionService.Setup(x => x.AddDivision(divisionDto[0]))
                    .ReturnsAsync(divisionDto[0]);

            var divisionController = new DivisionController(_divisionService.Object);

            var divisionResult = await divisionController.AddDivision(divisionDto[0]);

            Assert.NotNull(divisionResult);
            Assert.IsType<OkObjectResult>(divisionResult);

        }
        [Fact]
        public async Task Updatedivision_ShouldOkReturns_WhenStatusIsOk()
        {
            var divisionDto = ApiTestData.GetDivisionDtoData();
            _divisionService.Setup(x => x.UpdateDivision(divisionDto[0]))
                .ReturnsAsync(divisionDto[0]);

            var divisionController = new DivisionController(_divisionService.Object);

            var divisionResult = await divisionController.UpdateDivision(divisionDto[0]);

            Assert.NotNull(divisionResult);
            Assert.IsType<OkObjectResult>(divisionResult);
        }
        [Fact]
        public async Task DeleteDivision_ShouldOkReturns_WhenStatusIsOk()
        {
            var divisionDto = ApiTestData.GetDivisionDtoData();

            _divisionService.Setup(x => x.DeleteDivision(It.IsAny<Guid>()));


            var divisionController = new DivisionController(_divisionService.Object);

            await divisionController.DeleteDivision(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
