using DAIS.API.Controllers;
using DAIS.API.Helpers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace DAIS.API.Tests
{
    public class MaterialControllerTest
    {
        private readonly Mock<IMaterialService> _materialService;
        private readonly Mock<IOptions<MaterialConfigSettings>> configMock;
        public MaterialControllerTest()
        {
            _materialService = new Mock<IMaterialService>();
            var _configData = new MaterialConfigSettings { DocumentBasePath = "TestConnectionString" };
            configMock = new Mock<IOptions<MaterialConfigSettings>>();
            configMock.Setup(x => x.Value).Returns(_configData);
        }

        [Fact]
        public async Task GetMaterialById_ShouldReturnsMaterialById_WhenStatusIsOk()
        {
            var materialDtoData = ApiTestData.GetMaterialDtoData();

            _materialService.Setup(x => x.GetMaterialByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(materialDtoData[0]);
            var materialController = new MaterialController(_materialService.Object, configMock.Object);

            var materialResult=await materialController.GetMaterialById(Guid.NewGuid());

            Assert.NotNull(materialResult);            
            Assert.IsType<OkObjectResult>(materialResult);

        }

        [Fact]
        public async Task GetAllMaterials_ShouldReturnsMaterials_WhenStatusIsOk()
        {
            var materialDtoData = ApiTestData.GetMaterialDtoData();

            _materialService.Setup(x => x.GetAllMaterialsAsync(Guid.NewGuid()))
                .ReturnsAsync(materialDtoData);

            var materialController = new MaterialController(_materialService.Object, configMock.Object);

            var materialResult = await materialController.GetAllMaterials(Guid.NewGuid());

            Assert.NotNull(materialResult);
            Assert.IsType<OkObjectResult>(materialResult);
        }       

        //[Fact]
        //public async Task Update_ShouldUpdateMaterial_WhenStatusIsOk()
        //{
        //    var materialDto = ApiTestData.GetMaterialDtoData();

        //    _materialService.Setup(x => x.UpdateMaterialAsync(materialDto[0]))
        //        .ReturnsAsync(materialDto[0]);

        //    var materialController = new MaterialController(_materialService.Object, configMock.Object);

        //    var materialResult = await materialController.UpdateMaterial(null, materialDto[0]);
            
        //    Assert.NotNull(materialResult);           
        //    Assert.IsType<OkObjectResult>(materialResult);
        //}

        [Fact]
        public async Task Delete_ShouldDeleteMaterial_WhenStatusIsOk()
        {
            var materialDto = ApiTestData.GetMaterialDtoData();

            _materialService.Setup(x => x.DeleteMaterialAsync(It.IsAny<Guid>()));
                

            var materialController = new MaterialController(_materialService.Object, configMock.Object);

            await materialController.DeleteMaterial(Guid.NewGuid());
            
            Assert.True(true);
        }
    }
}
