using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DAIS.CoreBusiness.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IGenericRepository<Category>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CategoryService>> _mockLogger;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<Category>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CategoryService>>();
            _service = new CategoryService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(Skip = "For time being")]
        public async Task AddCategory_Success()
        {
            // Arrange
            var categoryDto = new CategoryDto 
            { 
                Id = Guid.NewGuid(),
                CategoryName = "Test Category",
                CategoryCode = "TEST001",
                ProjectId = Guid.NewGuid(),
                MaterialTypeId = Guid.NewGuid()
            };
            var category = new Category();
            
            _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _mockRepo.Setup(r => r.Add(It.IsAny<Category>())).ReturnsAsync(category);

            // Act
            var result = await _service.AddCategory(categoryDto);

            // Assert
            Assert.Equal(categoryDto, result);
            _mockRepo.Verify(r => r.Add(category), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task DeleteCategory_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category();
            
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync(category);
            _mockRepo.Setup(r => r.Remove(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCategory(id);

            // Assert
            _mockRepo.Verify(r => r.Remove(category), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task GetAllCategory_Success()
        {
            // Arrange
            var categories = new List<Category> 
            { 
                new Category 
                { 
                    Id = Guid.NewGuid(),
                    CategoryName = "Test Category 1",
                    MaterialType = new MaterialType { TypeName = "Type 1" }
                },
                new Category 
                { 
                    Id = Guid.NewGuid(),
                    CategoryName = "Test Category 2",
                    MaterialType = new MaterialType { TypeName = "Type 2" }
                }
            };

            var mockQueryable = categories.AsQueryable();
            var mockDbSet = new Mock<DbSet<Category>>();

            _mockRepo.Setup(r => r.Query()).Returns(mockQueryable);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                      .Returns<Category>(c => new CategoryDto 
                      { 
                          Id = c.Id,
                          CategoryName = c.CategoryName,
                          MaterialTypeName = c.MaterialType.TypeName
                      });

            // Act
            var result = await _service.GetAllCategory();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Type 1", result[0].MaterialTypeName);
            Assert.Equal("Type 2", result[1].MaterialTypeName);
        }

        [Fact(Skip = "For time being")]
        public async Task GetCategoryTypeById_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category 
            { 
                Id = id,
                CategoryName = "Test Category",
                MaterialType = new MaterialType { TypeName = "Test Type" },
                Project = new Project { ProjectName = "Test Project" }
            };

            var mockQueryable = new List<Category> { category }.AsQueryable();
            
            _mockRepo.Setup(r => r.Query()).Returns(mockQueryable);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
                      .Returns(new CategoryDto 
                      { 
                          Id = category.Id,
                          CategoryName = category.CategoryName,
                          MaterialTypeName = category.MaterialType.TypeName
                      });

            // Act
            var result = await _service.GetCategoryTypeById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test Category", result.CategoryName);
            Assert.Equal("Test Type", result.MaterialTypeName);
        }

        [Fact(Skip = "For time being")]
        public async Task UpdateCategory_Success()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "Updated Category",
                CategoryCode = "UPD001",
                ProjectId = Guid.NewGuid(),
                MaterialTypeId = Guid.NewGuid()
            };
            var category = new Category();
            
            _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _mockRepo.Setup(r => r.Update(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateCategory(categoryDto);

            // Assert
            Assert.Equal(categoryDto, result);
            _mockRepo.Verify(r => r.Update(category), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public void GetCategoryIdByName_Success()
        {
            // Arrange
            var name = "Test Category";
            var category = new Category { CategoryName = name };
            var categoryDto = new CategoryDto { CategoryName = name };
            var queryable = new List<Category> { category }.AsQueryable();
            
            _mockRepo.Setup(r => r.Query()).Returns(queryable);
            _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);

            // Act
            var result = _service.GetCategoryIdByName(name);

            // Assert
            Assert.Equal(categoryDto, result);
            Assert.Equal(name, result.CategoryName);
        }

        [Fact(Skip = "For time being")]
        public async Task AddCategory_ThrowsException()
        {
            // Arrange
            var categoryDto = new CategoryDto();
            var category = new Category();
            var expectedException = new Exception("Test exception");
            
            _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _mockRepo.Setup(r => r.Add(category)).ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddCategory(categoryDto));
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact(Skip = "For time being")]
        public async Task DeleteCategory_ThrowsException_WhenCategoryNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteCategory(id));
        }

        [Fact(Skip = "For time being")]
        public void GetCategoryIdByName_ReturnsEmpty_WhenCategoryNotFound()
        {
            // Arrange
            var name = "Nonexistent Category";
            var queryable = new List<Category>().AsQueryable();
            
            _mockRepo.Setup(r => r.Query()).Returns(queryable);

            // Act
            var result = _service.GetCategoryIdByName(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.Id);
        }
    }
}
