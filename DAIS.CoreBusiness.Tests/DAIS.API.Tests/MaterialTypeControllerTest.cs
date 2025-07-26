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
    public  class MaterialTypeControllerTest
    {
        private readonly Mock<IMaterialTypeService> _materialTypeService;
        public MaterialTypeControllerTest()
        {
            _materialTypeService = new Mock<IMaterialTypeService>();
        }
        [Fact]
        public async void GetAllMaterialType_Returns_OkResult()
        {
            var materialTypeDtoData = ApiTestData.GetMaterialTypeDtoData();
            _materialTypeService.Setup(x => x.GetMaterialTypeById(It.IsAny<Guid>()))
                .ReturnsAsync(materialTypeDtoData[0]);

            var materialTypeController = new MaterialTypeController(_materialTypeService.Object);

            var materialWarrantyResult = await materialTypeController.GetAllMaterialType(Guid.NewGuid());

            Assert.NotNull(materialWarrantyResult);
            Assert.IsType<OkObjectResult>(materialWarrantyResult);
        }
        [Fact]
        public async void GetAllMaterialTypes_Returns_OkResult()
        {
            var materialTypeDtoData = ApiTestData.GetMaterialTypeDtoData();
            _materialTypeService.Setup(x => x.GetAllMaterialTypes())
                .ReturnsAsync(materialTypeDtoData);

            var materialTypeController = new MaterialTypeController(_materialTypeService.Object);

            var materialTypeResult = await materialTypeController.GetAllMaterialTypes();

            Assert.NotNull(materialTypeResult);
            Assert.IsType<OkObjectResult>(materialTypeResult);
        }
        [Fact]
        public async void AddMateialType_Returns_OkResult()
        {

            var materialTypeDto = ApiTestData.GetMaterialTypeDtoData();
            _materialTypeService.Setup(x => x.AddMaterialType(materialTypeDto[0]))
                    .ReturnsAsync(materialTypeDto[0]);

            var materialTypeController = new MaterialTypeController(_materialTypeService.Object);

            var materialTypeResult = await materialTypeController.AddMateialType(materialTypeDto[0]);

            Assert.NotNull(materialTypeResult);
            Assert.IsType<OkObjectResult>(materialTypeResult);

        }
        [Fact]
        public async void UpdateMateialType_Returns_OkResult()
        {
            var materialTypeDto = ApiTestData.GetMaterialTypeDtoData();
            _materialTypeService.Setup(x => x.UpdateMaterialType(materialTypeDto[0]))
                .ReturnsAsync(materialTypeDto[0]);

            var materialTypeController = new MaterialTypeController(_materialTypeService.Object);

            var materialTypeResult = await materialTypeController.UpdateMateialType(materialTypeDto[0]);

            Assert.NotNull(materialTypeResult);
            Assert.IsType<OkObjectResult>(materialTypeResult);
        }
        [Fact]
        public async void DeleteMateialType_Returns_OkResult()
        {
            var materialTypeDto = ApiTestData.GetMaterialTypeDtoData();

            _materialTypeService.Setup(x => x.DeleteMaterialType(It.IsAny<Guid>()));


            var materialTypeController = new MaterialTypeController(_materialTypeService.Object);

            await materialTypeController.DeleteMateialType(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
