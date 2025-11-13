using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class DesignDocumentService : IDesignDocumentService
    {
        private IGenericRepository<DesignDocument> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DesignDocumentService> _logger;

        public DesignDocumentService(IGenericRepository<DesignDocument> genericRepo, IMapper mapper, ILogger<DesignDocumentService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<DesignDocumentDto> AddDesignDocumentAsync(DesignDocumentDto designDocumentDto)
        {
            _logger.LogInformation("DesignDocumentService:AddDesignDocumentAsync:Method Start");

            try
            {
                if (!string.IsNullOrEmpty(designDocumentDto.DesignDocumentName))
                {
                    designDocumentDto.DesignDocumentName = designDocumentDto.DesignDocumentName.ToUpper();
                }
                var document = _mapper.Map<DesignDocument>(designDocumentDto);
                await _genericRepo.Add(document);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DesignDocumentService:AddDesignDocumentAsync:Method End");
            return designDocumentDto;
        }

        public async Task DeleteDesignDocumentAsync(Guid Id)
        {
            _logger.LogInformation("DesignDocumentService: DeleteDesignDocumentAsync:Method End");
            try
            {
                var designDocument = await _genericRepo.GetById(Id);

                await _genericRepo.Remove(designDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("DesignDocumentService:DeleteDesignDocumentAsync:Method End");
        }

        public async Task<List<DesignDocumentDto>> GetAllDesignlDocumentAsync()
        {

            _logger.LogInformation("designDocumentService:GetAlldesignDocument:Method Start");
            List<DesignDocumentDto> DesignDocumentDtosList = new List<DesignDocumentDto>();
            try
            {
                var designDocumentList = await _genericRepo.Query().
                            Include(x => x.WorkPackage)
                            .ThenInclude(x=>x.Project)
                            .ToListAsync().ConfigureAwait(false);
                foreach (var designDocument in designDocumentList)
                {
                    var designDocumentDto = _mapper.Map<DesignDocumentDto>(designDocument);
                    DesignDocumentDtosList.Add(designDocumentDto);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("designDocumentService: GetAlldesignDocument:Method End");
            return DesignDocumentDtosList;
        }

        public async Task<List<DesignDocumentDto>> GetAllDesignlDocumentsByProjectWorkpackageAsync(Guid projectId,Guid ?workPackageId)
        {

            _logger.LogInformation("designDocumentService:GetAlldesignDocument:Method Start");
            List<DesignDocumentDto> DesignDocumentDtosList = new List<DesignDocumentDto>();
            try
            {
                var designDocumentList = await _genericRepo.Query() 
                        . Where(x=>x.WorkPackageId== workPackageId)
                            .Include(x => x.WorkPackage)
                            .ThenInclude(x=>x.Project)
                            .ToListAsync();
                
                
                foreach (var designDocument in designDocumentList)
                {
                    var designDocumentDto = _mapper.Map<DesignDocumentDto>(designDocument);
                    DesignDocumentDtosList.Add(designDocumentDto);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("designDocumentService: GetAlldesignDocument:Method End");
            return DesignDocumentDtosList;
        }

        public async Task<DesignDocumentDto> UpdateDesignDocumentAsync(DesignDocumentDto designDocumentDto)
        {
            _logger.LogInformation("DesignDocumentService: UpdateDesignDocument:Method Start");
            try
            {
                var existingDesignDocument = await _genericRepo.GetById(designDocumentDto.Id);
                if (existingDesignDocument != null)
                {
                    if (!string.IsNullOrEmpty(designDocumentDto.DocumentFileName))
                    {
                        existingDesignDocument.DocumentFileName = designDocumentDto.DocumentFileName;
                    }
                    existingDesignDocument.UpdatedDate = DateTime.Now;
                    existingDesignDocument.DesignDocumentName =designDocumentDto.DesignDocumentName;
                    

                }
              
                await _genericRepo.Update(existingDesignDocument);
    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DesignDocumentService:UpdateDesignDocument:Method End");
            return designDocumentDto;
        }

        public async Task<DesignDocumentDto> GetDesignDocumentByIdAsync(Guid Id)
        {
            _logger.LogInformation("GetDesignDocumentByIdService:GetDesignDocumentById:Method Start");
            DesignDocumentDto designDocumentDto = new DesignDocumentDto();
            try
            {
                var designDocument = await _genericRepo.Query()
                 .Include(x => x.WorkPackage).
                 ThenInclude(x=>x.Project)
                .FirstOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);
                designDocumentDto = _mapper.Map<DesignDocumentDto>(designDocument);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DesignDocumentService:GetdesignDocumentByIdAsync:Method End");
            return designDocumentDto;
        }

        public DesignDocumentDto GetDesignDocumentIdByName(string name)
        {
            _logger.LogInformation("DesignDocumentService:DesignDocumentIdByName:Method Start");
            DesignDocumentDto designDocumentDto = new DesignDocumentDto();
            try
            {
                var designDocument = _genericRepo.Query()
                 .FirstOrDefault(x => x.DesignDocumentName.ToLower() == name.ToLower());
                designDocumentDto = _mapper.Map<DesignDocumentDto>(designDocument);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DesignDocumentService::DesignDocumentIdByName:Method End");
            return designDocumentDto;
        }
    }
}

