using Aspose.Cells;
using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using DAIS.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialMaintenanceService : IMaterialMaintenanceService
    {
        private readonly IGenericRepository<MaterialMaintenance> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialMaintenanceService> _logger;
        public MaterialMaintenanceService(IGenericRepository<MaterialMaintenance> genericRepo, IMapper mapper, ILogger<MaterialMaintenanceService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            
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
                var materialMaintenance= _mapper.Map<MaterialMaintenance>(id);
                await _genericRepo.Remove(materialMaintenance);

                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMaintenanceService:DeleteMaterialMaintenaceAsync:Method End");
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
                    if(existingMaintenance.MaintenanceDocument!=null)
                    {
                        existingMaintenance.MaintenanceDocument =materialMaintenaceDto.MaintenanceDocument;
                    }
                    existingMaintenance.MaintenanceStartDate = materialMaintenaceDto.MaintenanceStartDate;
                    existingMaintenance.MaintenanceEndDate = materialMaintenaceDto.MaintenanceEndDate;
                }
                var maintenance = _mapper.Map<MaterialMaintenance>(materialMaintenaceDto);
                await _genericRepo.Update(maintenance);
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
