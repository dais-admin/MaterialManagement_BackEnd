using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace DAIS.CoreBusiness.Services
{
    public class DocumnetTypeService : IDocumentTypeService
    {
        private readonly IGenericRepository<DocumentType> _genericRepository;
        private IMapper _mapper;
        private readonly ILogger<DocumentType> _logger;
        public DocumnetTypeService(IGenericRepository<DocumentType> genericRepository,IMapper mapper, ILogger<DocumentType> logger)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<DocumentMasterDto> AddDocumentAsync(DocumentMasterDto documentMasterDto)
        {
            _logger.LogInformation("DocumnetTypeService:AddDocumentAsync:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(documentMasterDto.DocumentName))
                {
                    documentMasterDto.DocumentName = documentMasterDto.DocumentName.ToUpper();
                }
                var document=_mapper.Map<DocumentType>(documentMasterDto);
                await _genericRepository.Add(document);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumnetTypeService:AddDocumentAsync:Method End");
            return documentMasterDto;
        }

        public async Task DeleteDocumentAsync(Guid id)
        {
            _logger.LogInformation("DocumnetTypeService:DeleteDocumentAsync:Method Start");
            try 
            {
                var document = await _genericRepository.GetById(id);
                await _genericRepository.Remove(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumnetTypeService:DeleteDocumentAsync:Method End");
        }

        public async Task<List<DocumentMasterDto>> GetAllDocumentAsync()
        {
            _logger.LogInformation("DocumnetTypeService:GetAllDocumentAsync:Method Start");
            List<DocumentMasterDto> documentMasterDtoList=new List<DocumentMasterDto>();
            try
            {
                var documentTypes = await _genericRepository.GetAll();
                foreach (var documentType in documentTypes)
                {
                    var documentMasterDto=_mapper.Map<DocumentMasterDto>(documentType);
                    documentMasterDtoList.Add(documentMasterDto);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("DocumnetTypeService:GetAllDocumentAsync:Method End");
            return documentMasterDtoList;
        }

        public async Task<DocumentMasterDto> GetDocumentByIdAsync(Guid id)
        {
            _logger.LogInformation("DocumnetTypeService:GetDocumentByIdAsync:Method Start");
            DocumentMasterDto documentMasterDto = new DocumentMasterDto();
            try
            {
                var documentType=await _genericRepository.GetById(id);
                 documentMasterDto= _mapper.Map<DocumentMasterDto>(documentType);
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumnetTypeService:GetDocumentByIdAsync:Method End");
            return documentMasterDto;
        }
     
        public async Task<DocumentMasterDto> UpdateDocumentAsync(DocumentMasterDto documentMasterDto)
        {
            _logger.LogInformation("DocumnetTypeService:UpdateDocumentAsync:Method Start");
            try
            {
               
                var document = _mapper.Map<DocumentType>(documentMasterDto);
                await _genericRepository.Update(document);
          
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DocumnetTypeService:UpdateDocumentAsync:Method End");
            return documentMasterDto;
        }
    }
}
