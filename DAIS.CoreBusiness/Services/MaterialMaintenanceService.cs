using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialMaintenanceService : IMaterialMaintenanceService
    {
        private readonly IGenericRepository<MaterialMaintenance> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialMaintenanceService> _logger;
        private readonly IFileManagerService _fileManager;
        public MaterialMaintenanceService(IGenericRepository<MaterialMaintenance> genericRepo, IMapper mapper, ILogger<MaterialMaintenanceService> logger, IFileManagerService fileManager)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        }
        public async Task<MaterialMaintenaceDto> AddMaterialMaintenanceAsync(MaterialMaintenaceDto materialMaintenaceDto)
        {
            _logger.LogInformation("MaterialMaintenanceService:AddMaterialMaintenanceAsync:Method Start");
            try
            {
                var maintenance = _mapper.Map<MaterialMaintenance>(materialMaintenaceDto);
                await _genericRepo.Add(maintenance);

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:AddMaterialMaintenanceAsync:Method End");
            return materialMaintenaceDto;
        }

        public async Task DeleteMaterialMaintenaceAsync(Guid id)
        {
            _logger.LogInformation("MaterialMaintenanceService:DeleteMaterialMaintenaceAsync:Method Start");

            try
            {
                var materialMaintenance = await _genericRepo.GetById(id);

                if (materialMaintenance == null)
                {
                    _logger.LogWarning($"MaterialMaintenance with id {id} not found.");
                    return;
                }

                // Delete files/directories if documents exist
                if (!string.IsNullOrWhiteSpace(materialMaintenance.MaintenanceDocument))
                {
                    var files = materialMaintenance.MaintenanceDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Delete DB record
                await _genericRepo.Remove(materialMaintenance);

                _logger.LogInformation("MaterialMaintenanceService:DeleteMaterialMaintenaceAsync:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteMaterialMaintenaceAsync");
                throw;
            }
        }


        public async Task<List<MaterialMaintenaceDto>> GetAllMaterialMaintenacesAsync()
        {
            _logger.LogInformation("MaterialMaintenanceService:GetAllMaterialMaintenacesAsync:Method Start");
            List<MaterialMaintenaceDto> materialMaintenanceDtoList = new List<MaterialMaintenaceDto>();
            try
            {
                 var  materialMaintenanceList = await _genericRepo.GetAll();
              

                foreach (var maintenance in materialMaintenanceList)
                {
                    var materialMaintenanceDto = _mapper.Map<MaterialMaintenaceDto>(maintenance);
                    materialMaintenanceDtoList.Add(materialMaintenanceDto);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:GetAllMaterialMaintenacesAsync:Method End");
            return materialMaintenanceDtoList;
        }

        public  async Task<MaterialMaintenaceDto> GetMaintenanceByMaterialIdAsync(Guid materialId)
        {
            _logger.LogInformation("MaterialMaintenanceService:GetWarrantyByMaterialIdAsync:Method Start");
            MaterialMaintenaceDto maintenaceDto = new MaterialMaintenaceDto();              
            try
            {

                var maintenance= await _genericRepo.Query()
                    .Where(x => x.MaterialId == materialId)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                maintenaceDto = _mapper.Map<MaterialMaintenaceDto>(maintenance);

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:GetMaintenanceByMaterialIdAsync:Method End");
            return maintenaceDto;
        }

        public async Task<MaterialMaintenaceDto> GetMaterialMaintenaceByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialMaintenanceService:GetMaterialMaintenaceByIdAsync:Method Start");
               MaterialMaintenaceDto maintenaceDto = new MaterialMaintenaceDto();
            try
            {
                var materialMaintenance = await _genericRepo.GetById(id);
                maintenaceDto= _mapper.Map<MaterialMaintenaceDto>(materialMaintenance);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:GetMaterialMaintenaceByIdAsync:Method End");
            return maintenaceDto;

        }

        public async Task<MaterialMaintenaceDto> UpdateMaterialMaintenaceAsync(MaterialMaintenaceDto materialMaintenaceDto)
        {
            _logger.LogInformation("MaterialMaintenanceService:UpdateMaterialMaintenaceAsync:Method Start");
            try
            {
                var existingMaintenance = await _genericRepo.GetById(materialMaintenaceDto.Id);
                if (existingMaintenance != null)
                {
                    if (!string.IsNullOrEmpty(existingMaintenance.MaintenanceDocument))
                    {
                        existingMaintenance.MaintenanceDocument =materialMaintenaceDto.MaintenanceDocument;
                    }
                    existingMaintenance.MaintenanceStartDate = materialMaintenaceDto.MaintenanceStartDate;
                    existingMaintenance.MaintenanceEndDate = materialMaintenaceDto.MaintenanceEndDate;
                }
                
                await _genericRepo.Update(existingMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:UpdateMaterialMaintenaceAsync:Method End");
            return materialMaintenaceDto;
        }
    }
}
