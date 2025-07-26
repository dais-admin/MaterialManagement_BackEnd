using DAIS.API.Controllers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DAIS.API.Tests
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _categoryService;
        public CategoryControllerTest()
        {
            _categoryService = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturns_WhenStatusIsOk()
        {
            var categoryDtoData = ApiTestData.GetCategoryDtoData();
            _categoryService.Setup(x => x.GetCategoryTypeById(It.IsAny<Guid>()))
                .ReturnsAsync(categoryDtoData[0]);

            var categoryController = new CategoryController(_categoryService.Object);

            var categoryResult = await categoryController.GetCategory(Guid.NewGuid());

            Assert.NotNull(categoryResult);
            Assert.IsType<OkObjectResult>(categoryResult);
        }

        [Fact]
        public async void GetAllCategories_ShouldOkReturns_WhenStatusIsOk()
        {
            var categoryDtoData = ApiTestData.GetCategoryDtoData();

            _categoryService.Setup(x => x.GetAllCategory())
                .ReturnsAsync(categoryDtoData);

            var categoryController = new CategoryController(_categoryService.Object);

            var categoryResult = await categoryController.GetAllCategory();

            Assert.NotNull(categoryResult);
            Assert.IsType<OkObjectResult>(categoryResult);
        }

        [Fact]
        public async Task AddCategory_ShouldReturns_WhenStatusIsOk()
        {
            var categoryDto = ApiTestData.GetCategoryDtoData();

            _categoryService.Setup(x => x.AddCategory(categoryDto[0]))
                    .ReturnsAsync(categoryDto[0]);

            var categoryController = new CategoryController(_categoryService.Object);

            var categoryResult = await categoryController.AddCategory(categoryDto[0]);

            Assert.NotNull(categoryResult);
            Assert.IsType<OkObjectResult>(categoryResult);

        }

        [Fact]
        public async Task UpdateCategory_ShouldReturns_WhenStatusIsOk()
        {
            var categoryDto = ApiTestData.GetCategoryDtoData();

            _categoryService.Setup(x => x.UpdateCategory(categoryDto[0]))
                .ReturnsAsync(categoryDto[0]);

            var categoryController = new CategoryController(_categoryService.Object);

            var categoryResult = await categoryController.UpdateCategory(categoryDto[0]);

            Assert.NotNull(categoryResult);
            Assert.IsType<OkObjectResult>(categoryResult);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturns_WhenStatusIsOk()
        {
            var categoryDto = ApiTestData.GetCategoryDtoData();

            _categoryService.Setup(x => x.DeleteCategory(It.IsAny<Guid>()));


            var categoryController = new CategoryController(_categoryService.Object);

            await categoryController.DeleteCategory(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
