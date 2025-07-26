using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DAIS.CoreBusiness.Tests.Services
{
    public class AgencyServiceTests
    {
        private readonly Mock<IGenericRepository<Agency>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AgencyService>> _mockLogger;
        private readonly AgencyService _service;

        public AgencyServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<Agency>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AgencyService>>();
            _service = new AgencyService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact(Skip = "For time being")]
        public async Task AddAgency_Success()
        {
            // Arrange
            var agencyDto = new AgencyDto 
            { 
                Id = Guid.NewGuid().ToString(),
                AgencyCode = "TEST001",
                AgencyName = "Test Agency",
                AgencyType = "Test Type"
            };
            var agency = new Agency();
            
            _mockMapper.Setup(m => m.Map<Agency>(agencyDto)).Returns(agency);
            _mockRepo.Setup(r => r.Add(It.IsAny<Agency>())).ReturnsAsync(agency);

            // Act
            var result = await _service.AddAgency(agencyDto);

            // Assert
            Assert.Equal(agencyDto, result);
            _mockRepo.Verify(r => r.Add(agency), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task DeleteAgency_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var agency = new Agency();
            
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync(agency);
            _mockRepo.Setup(r => r.Remove(It.IsAny<Agency>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAgency(id);

            // Assert
            _mockRepo.Verify(r => r.Remove(agency), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task GetAgency_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var agency = new Agency();
            var agencyDto = new AgencyDto();
            
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync(agency);
            _mockMapper.Setup(m => m.Map<AgencyDto>(agency)).Returns(agencyDto);

            // Act
            var result = await _service.GetAgency(id);

            // Assert
            Assert.Equal(agencyDto, result);
            _mockRepo.Verify(r => r.GetById(id), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public void GetAgencyIdByName_Success()
        {
            // Arrange
            var name = "Test Agency";
            var agency = new Agency { AgencyName = name };
            var agencyDto = new AgencyDto();
            var queryable = new List<Agency> { agency }.AsQueryable();
            
            _mockRepo.Setup(r => r.Query()).Returns(queryable);
            _mockMapper.Setup(m => m.Map<AgencyDto>(It.IsAny<Agency>())).Returns(agencyDto);

            // Act
            var result = _service.GetAgencyIdByName(name);

            // Assert
            Assert.Equal(agencyDto, result);
        }

        [Fact(Skip = "For time being")]
        public async Task GetAllAgency_Success()
        {
            // Arrange
            var agencies = new List<Agency> { new Agency(), new Agency() };
            var agencyDtos = new List<AgencyDto> { new AgencyDto(), new AgencyDto() };
            
            _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(agencies);
            _mockMapper.Setup(m => m.Map<AgencyDto>(It.IsAny<Agency>())).Returns(agencyDtos[0]);

            // Act
            var result = await _service.GetAllAgency();

            // Assert
            Assert.Equal(agencyDtos.Count, result.Count);
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task UpdateAgency_Success()
        {
            // Arrange
            var agencyDto = new AgencyDto
            {
                Id = Guid.NewGuid().ToString(),
                AgencyCode = "TEST001",
                AgencyName = "Test Agency",
                AgencyType = "Test Type"
            };
            var agency = new Agency();
            
            _mockMapper.Setup(m => m.Map<Agency>(agencyDto)).Returns(agency);
            _mockRepo.Setup(r => r.Update(It.IsAny<Agency>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAgency(agencyDto);

            // Assert
            Assert.Equal(agencyDto, result);
            _mockRepo.Verify(r => r.Update(agency), Times.Once);
        }

        [Fact(Skip = "For time being")]
        public async Task AddAgency_ThrowsException()
        {
            // Arrange
            var agencyDto = new AgencyDto();
            var agency = new Agency();
            var expectedException = new Exception("Test exception");
            
            _mockMapper.Setup(m => m.Map<Agency>(agencyDto)).Returns(agency);
            _mockRepo.Setup(r => r.Add(agency)).ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddAgency(agencyDto));
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact(Skip = "For time being")]
        public async Task DeleteAgency_ThrowsException_WhenAgencyNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync((Agency?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAgency(id));
        }

        [Fact(Skip = "For time being")]
        public async Task GetAgency_ReturnsEmpty_WhenAgencyNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetById(id)).ReturnsAsync((Agency?)null);

            // Act
            var result = await _service.GetAgency(id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Id);
        }

        [Fact(Skip = "For time being")]
        public void GetAgencyIdByName_ReturnsEmpty_WhenAgencyNotFound()
        {
            // Arrange
            var name = "Nonexistent Agency";
            var queryable = new List<Agency>().AsQueryable();
            
            _mockRepo.Setup(r => r.Query()).Returns(queryable);

            // Act
            var result = _service.GetAgencyIdByName(name);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Id);
        }
    }
}
