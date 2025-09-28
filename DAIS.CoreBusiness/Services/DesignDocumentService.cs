using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var designDocument = _mapper.Map<DesignDocument>(designDocumentDto);
                await _genericRepo.Add(designDocument);

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
                            Include(x => x.Project).
                            Include(x => x.WorkPackage)
                            .ToListAsync().ConfigureAwait(false);
                foreach (var designDocument in designDocumentList)
                {
                    var designDocumentDto = _mapper.Map<DesignDocumentDto>(designDocumentList);
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

        public async Task<DesignDocumentDto> GetDesignDocumentByIdAsync(Guid Id)
        {
            _logger.LogInformation("MaterialSoftwareService:GetMaterialSoftwareByIdAsync:Method Start");
            DesignDocumentDto designDocumentDto = new DesignDocumentDto();
            try
            {
                var designDocument = await _genericRepo.GetById(Id);
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

        public async Task<DesignDocumentDto> UpdateDesignDocumentAsync(DesignDocumentDto designDocumentDto)
        {
            _logger.LogInformation("DesignDocumentService: UpdateDesignDocumentAsync:Method Start");
            try
            {
                var designDocument = _mapper.Map<DesignDocument>(designDocumentDto);
                await _genericRepo.Update(designDocument);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DesignDocumentService:AddDesignDocumentAsync:Method End");
            return designDocumentDto;
        }
    }
}
