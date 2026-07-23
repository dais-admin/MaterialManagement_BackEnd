using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class DocumentCategoryService : IDocumentCategoryService
    {
        private readonly IGenericRepository<DocumentCategory> _genericRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentCategory> _logger;

        public DocumentCategoryService(IGenericRepository<DocumentCategory> genericRepository, IMapper mapper, ILogger<DocumentCategory> logger)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DocumentCategoryDto> AddDocumentCategoryAsync(DocumentCategoryDto documentCategoryDto)
        {
            _logger.LogInformation("DocumentCategoryService:AddDocumentCategoryAsync:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(documentCategoryDto.CategoryName))
                {
                    documentCategoryDto.CategoryName = documentCategoryDto.CategoryName.ToUpper();
                }
                var documentCategory = _mapper.Map<DocumentCategory>(documentCategoryDto);
                await _genericRepository.Add(documentCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumentCategoryService:AddDocumentCategoryAsync:Method End");
            return documentCategoryDto;
        }

        public async Task DeleteDocumentCategoryAsync(Guid id)
        {
            _logger.LogInformation("DocumentCategoryService:DeleteDocumentCategoryAsync:Method Start");
            try
            {
                var documentCategory = await _genericRepository.GetById(id);
                if (documentCategory != null)
                {
                    await _genericRepository.Remove(documentCategory);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumentCategoryService:DeleteDocumentCategoryAsync:Method End");
        }

        public async Task<List<DocumentCategoryDto>> GetAllDocumentCategoryAsync()
        {
            _logger.LogInformation("DocumentCategoryService:GetAllDocumentCategoryAsync:Method Start");
            List<DocumentCategoryDto> dtoList = new List<DocumentCategoryDto>();
            try
            {
                var documentCategories = await _genericRepository.GetAll();
                foreach (var item in documentCategories)
                {
                    var dto = _mapper.Map<DocumentCategoryDto>(item);
                    dtoList.Add(dto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumentCategoryService:GetAllDocumentCategoryAsync:Method End");
            return dtoList;
        }

        public async Task<DocumentCategoryDto> GetDocumentCategoryByIdAsync(Guid id)
        {
            _logger.LogInformation("DocumentCategoryService:GetDocumentCategoryByIdAsync:Method Start");
            DocumentCategoryDto dto = new DocumentCategoryDto();
            try
            {
                var documentCategory = await _genericRepository.GetById(id);
                dto = _mapper.Map<DocumentCategoryDto>(documentCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumentCategoryService:GetDocumentCategoryByIdAsync:Method End");
            return dto;
        }

        public async Task<DocumentCategoryDto> UpdateDocumentCategoryAsync(DocumentCategoryDto documentCategoryDto)
        {
            _logger.LogInformation("DocumentCategoryService:UpdateDocumentCategoryAsync:Method Start");
            try
            {
                var documentCategory = _mapper.Map<DocumentCategory>(documentCategoryDto);
                await _genericRepository.Update(documentCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumentCategoryService:UpdateDocumentCategoryAsync:Method End");
            return documentCategoryDto;
        }
    }
}
