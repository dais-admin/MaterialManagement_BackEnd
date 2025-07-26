
using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialDocumentService : IMaterialDocumentService
    {
        private readonly IGenericRepository<MaterialDocument> _genericRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialDocument> _logger;
        public MaterialDocumentService(IGenericRepository<MaterialDocument> genericRepository,IMapper mapper, ILogger<MaterialDocument> logger)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<MaterialDocumentDto> AddMaterialDocumentAsync(MaterialDocumentDto materialDocumentDto)
        {
            _logger.LogInformation("MaterialDocumentService:AddMaterialDocumentAsync:Method Start");
            try
            {
                materialDocumentDto.CreatedDate = DateTime.UtcNow;
                var materialDocument=_mapper.Map<MaterialDocument>(materialDocumentDto);
                var dbEntity=await _genericRepository.Add(materialDocument);
                materialDocumentDto.Id= dbEntity.Id;
                materialDocumentDto.DocumentName = dbEntity.DocumentType?.DocumentName;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialDocumentService:AddMaterialDocumentAsync:Method End");
            return materialDocumentDto;
        }

        public async Task<MaterialDocumentDto> DeleteMaterialDocumentAsync(Guid id)
        {
            _logger.LogInformation("MaterialDocumentService:DeleteMaterialDocumentAsync:Method Start");
            MaterialDocumentDto materialDocumentDto=new MaterialDocumentDto();
            try
            {
                var documentToDeleted=await _genericRepository.GetById(id);
                
                if (documentToDeleted != null)
                {
                    documentToDeleted.IsDeleted = true;
                    await _genericRepository.Update(documentToDeleted);
                }
                materialDocumentDto = _mapper.Map<MaterialDocumentDto>(documentToDeleted);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialDocumentService:DeleteMaterialDocumentAsync:Method End");
            return materialDocumentDto;
        }

        public async Task<List<MaterialDocumentDto>> GetAllMaterialDocumentAsync()
        {
            _logger.LogInformation("MaterialDocumentService:GetAllMaterialDocumentAsync:Method Start");
            List<MaterialDocumentDto> materialDocumentDtoList = new List<MaterialDocumentDto>();
            try
            {
                var materialDocuments = await _genericRepository.Query().
                            Include(x => x.DocumentType)
                            .ToListAsync().ConfigureAwait(false);
                foreach (var materialDocument in materialDocuments)
                {                  
                    var assetDocumentDto=_mapper.Map<MaterialDocumentDto>(materialDocument);

                    materialDocumentDtoList.Add(assetDocumentDto);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialDocumentService:GetAllMaterialDocumentAsync:Method End");
            return materialDocumentDtoList;
        }
        public async Task<List<MaterialDocumentDto>> GetAllMaterialDocumentByMaterialIdAsync(Guid materialId)
        {
            _logger.LogInformation("MaterialDocumentService:GetAllMaterialDocumentByMaterialIdAsync:Method Start");
            List<MaterialDocumentDto> materialDocumentDtoList = new List<MaterialDocumentDto>();
            try
            {
                var materialDocuments = await _genericRepository.Query().
                    Include(x=> x.DocumentType)
                    .ToListAsync().ConfigureAwait(false);
                foreach( var materialDocument in materialDocuments.Where(x=>x.MaterialId==materialId))
                {
                    var materialDocumentDto = _mapper.Map<MaterialDocumentDto>(materialDocument);
                    materialDocumentDto.DocumentName = materialDocument.DocumentType.DocumentName;
                    materialDocumentDtoList.Add(materialDocumentDto);
                }
                      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialDocumentService:GetAllMaterialDocumentByMaterialIdAsync:Method End");
            return materialDocumentDtoList;
        }
        public async Task<MaterialDocumentDto> GetMaterialDocumentByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialDocumentService:GetMaterialDocumentByIdAsync:Method Start");
            MaterialDocumentDto materialDocumentDto = new MaterialDocumentDto();
            try
            {
                var materialDocument = await _genericRepository.GetById(id);
                materialDocumentDto = _mapper.Map<MaterialDocumentDto>(materialDocumentDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation(" MaterialDocumentService:GetMaterialDocumentByIdAsync:Method End");
            return materialDocumentDto;
        }

        public async Task<MaterialDocumentDto> UpdateMaterialDocumentAsync(MaterialDocumentDto materialDocumentDto)
        {
            _logger.LogInformation(" MaterialDocumentService:UpdateMaterialDocumentAsync:Method Start");
            try
            {
                var materialDocument = _mapper.Map<MaterialDocument>(materialDocumentDto);
                await _genericRepository.Update(materialDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation(" MaterialDocumentService:UpdateMaterialDocumentAsync:Method End");
            return materialDocumentDto;
        }
       
    }
}
