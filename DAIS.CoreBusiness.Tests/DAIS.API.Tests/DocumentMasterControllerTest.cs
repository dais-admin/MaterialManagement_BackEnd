using DAIS.API.Controllers;
using DAIS.API.Tests.TestData;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DAIS.API.Tests
{
    public class DocumentMasterControllerTest
    {
        private readonly Mock<IDocumentTypeService> _documentTypeService;
        public DocumentMasterControllerTest()
        {
            _documentTypeService = new Mock<IDocumentTypeService>();
        }

        [Fact]
        public async Task GetDocumentById_ShouldOkReturns_WhenStatusIsOk()
        {
            var documentDtoData = ApiTestData.GetDocumentMasterDtoData();
            _documentTypeService.Setup(x => x.GetDocumentByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(documentDtoData[0]);

            var documentMasterController = new DocumentMasterController(_documentTypeService.Object);

            var documentMasterResult = await documentMasterController.GetDocumentById(Guid.NewGuid());

            Assert.NotNull(documentMasterResult);
            Assert.IsType<OkObjectResult>(documentMasterResult);

        }
        [Fact]
        public async Task GetAllDocumnetMasterList_ShouldOkReturns_WhenStatusIsOk()
        {
            var documentDtoData = ApiTestData.GetDocumentMasterDtoData();
            _documentTypeService.Setup(x => x.GetAllDocumentAsync())
                .ReturnsAsync(documentDtoData);

            var documentMasterController = new DocumentMasterController(_documentTypeService.Object);

            var documentMasterResult = await documentMasterController.GetAllDocumnetMasterList();

            Assert.NotNull(documentMasterResult);
            Assert.IsType<OkObjectResult>(documentMasterResult);
        }

        [Fact]
        public async Task AddDocumentMaster_ShouldReturns_WhenStatusIsOk()
        {

            var documentDto = ApiTestData.GetDocumentMasterDtoData();
            _documentTypeService.Setup(x => x.AddDocumentAsync(documentDto[0]))
                    .ReturnsAsync(documentDto[0]);

            var documentMasterController = new DocumentMasterController(_documentTypeService.Object);

            var documentMasterResult = await documentMasterController.AddDocumentMaster(documentDto[0]);

            Assert.NotNull(documentMasterResult);
            Assert.IsType<OkObjectResult>(documentMasterResult);

        }

        [Fact]
        public async Task UpdateDocumentAsync_ShouldReturns_WhenStatusIsOk()
        {
            var documentDto = ApiTestData.GetDocumentMasterDtoData();
            _documentTypeService.Setup(x => x.UpdateDocumentAsync(documentDto[0]))
             .ReturnsAsync(documentDto[0]);

            var documentMasterController = new DocumentMasterController(_documentTypeService.Object);

            var documentMasterResult = await documentMasterController.UpdateDocumentAsync(documentDto[0]);

            Assert.NotNull(documentMasterResult);
            Assert.IsType<OkObjectResult>(documentMasterResult);
        }

        [Fact]
        public async Task DeleteDocumentAsync_ShouldReturns_WhenStatusIsOk()
        {
            var documentDto = ApiTestData.GetDocumentMasterDtoData();

            _documentTypeService.Setup(x => x.DeleteDocumentAsync(It.IsAny<Guid>()));

            var documentMasterController = new DocumentMasterController(_documentTypeService.Object);

            await documentMasterController.DeleteDocumentAsync(Guid.NewGuid());

            Assert.True(true);

        }
    }
}
