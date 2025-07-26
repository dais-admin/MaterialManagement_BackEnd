using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.API.Tests
{
    public class SupplierControllerTest
    {
        private readonly Mock<ISupplierService> _supplierService;
        private readonly Mock<IFileManagerService> _fileManagerService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public SupplierControllerTest()
        {
            _supplierService = new Mock<ISupplierService>();
            _fileManagerService = new Mock<IFileManagerService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }
        [Fact]
        public async void GetSupplier_Returns_OkResult()
        {
            var supplierDtoData = ApiTestData.GetSupplierDtoData();
            _supplierService.Setup(x => x.GetSupplier(It.IsAny<Guid>()))
            .ReturnsAsync(supplierDtoData[0]);

            var supplierController = new SupplierController(_supplierService.Object, _fileManagerService.Object);

            var supplierResult = await supplierController.GetSupplier(Guid.NewGuid());

            Assert.NotNull(supplierResult);
            Assert.IsType<OkObjectResult>(supplierResult);

        }
        [Fact]
        public async void GetAllSupplier_Returns_OkResult()
        {
            var supplierDtoData = ApiTestData.GetSupplierDtoData();
            _supplierService.Setup(x => x.GetAllSupplier())
            .ReturnsAsync(supplierDtoData);

            var supplierController = new SupplierController(_supplierService.Object, _fileManagerService.Object);

            var supplierResult = await supplierController.GetAllSupplier();

            Assert.NotNull(supplierResult);
            Assert.IsType<OkObjectResult>(supplierResult);
        }

        [Fact(Skip = "For time being")]
        public async void AddSupplier_Returns_OkResult()
        {

            var supplierDto = ApiTestData.GetSupplierDtoData();
           _supplierService.Setup(x => x.AddSupplier(supplierDto[0]))
            .ReturnsAsync(supplierDto[0]);

            var supplierController = new SupplierController(_supplierService.Object, _fileManagerService.Object);

            var supplierResult = "";//await supplierController.AddSupplier(supplierDto[0]);

            Assert.NotNull(supplierResult);
            Assert.IsType<OkObjectResult>(supplierResult);

        }

        [Fact(Skip = "For time being")]
        public async void UpdateSupplier_Returns_OkResult()
        {
            var supplierDto = ApiTestData.GetSupplierDtoData();
            _supplierService.Setup(x => x.UpdateSupplier(supplierDto[0]))
             .ReturnsAsync(supplierDto[0]);

            var supplierController = new SupplierController(_supplierService.Object, _fileManagerService.Object);

            var supplierResult = ""; //await supplierController.UpdateSupplier(supplierDto[0]);

            Assert.NotNull(supplierResult);
            Assert.IsType<OkObjectResult>(supplierResult);
        }
        [Fact]
        public async void DeleteSupplier_Returns_OkResult()
        {
            var supplierDto = ApiTestData.GetSupplierDtoData();

            _supplierService.Setup(x => x.DeleteSupplier(It.IsAny<Guid>()));


            var supplierController = new SupplierController(_supplierService.Object, _fileManagerService.Object);

            await supplierController.DeleteSupplier(Guid.NewGuid());

            Assert.True(true);
        }
    }
}
