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
    public class MaterialHardwareService : IMaterialHardwareService
    {
        private IGenericRepository<MaterialHardware> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialHardwareService> _logger;
        public MaterialHardwareService(IGenericRepository<MaterialHardware> genericRepo,IMapper mapper, ILogger<MaterialHardwareService> logger) 
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        
        }
        public  async Task<MaterialHardwareDto> AddMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto)
        {
            _logger.LogInformation("MaterialHardwareService:AddMaterialHardwareAsync:Method Start");
            try
            {
                var materialHardware = _mapper.Map<MaterialHardware>(materialHardwareDto);
                await _genericRepo.Add(materialHardware);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:AddMaterialHardwareAsync:Method End");
            return materialHardwareDto;
        }
        public async  Task<MaterialHardwareDto> UpdateMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto)
        {
            _logger.LogInformation("MaterialHardwareService:UpdateMaterialHardwareAsync:Method Start");
            try
            {
                var existingHardwareDocument = await _genericRepo.GetById(materialHardwareDto.Id);
                if (existingHardwareDocument != null)
                {
                    if (existingHardwareDocument.HardwareDocument != null)
                    {
                        existingHardwareDocument.HardwareDocument = materialHardwareDto.HardwareDocument;
                    }
                    existingHardwareDocument.UpdatedDate = DateTime.Now;
                    existingHardwareDocument.HarwareName = materialHardwareDto.HarwareName;
                    existingHardwareDocument.Chipset = materialHardwareDto.Chipset;
                    existingHardwareDocument.NetworkDetails = materialHardwareDto.NetworkDetails;
                    existingHardwareDocument.DateOfManufacturer = materialHardwareDto.DateOfManufacturer;
                    existingHardwareDocument.DiskDetails = materialHardwareDto.DiskDetails;
                    existingHardwareDocument.Remarks = materialHardwareDto.Remarks;
                    existingHardwareDocument.Quantity = materialHardwareDto.Quantity;
                }

                await _genericRepo.Update(existingHardwareDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:UpdateMaterialHardwareAsync:Method End");
            return materialHardwareDto;
        }
        public async Task<MaterialHardwareDto> GetMaterialHardwareByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialHardwareService:GetMaterialHardwareByIdAsync:Method Start");
            MaterialHardwareDto materialHardwareDto= new MaterialHardwareDto();
            try
            {
                var materialHardware = await _genericRepo.GetById(id);
                materialHardware = _mapper.Map<MaterialHardware>(materialHardware);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetMaterialHardwareByIdAsync:Method End");
            return materialHardwareDto;
        }
        public async Task<List<MaterialHardwareDto>> GetAllMaterialHardware()
        {
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method Start");
            List<MaterialHardwareDto> materialHardwareDtosList = new List<MaterialHardwareDto>();
            try
            {
                var materialHardwareList = await _genericRepo.Query()
                    .Include(x => x.Supplier)
                    .Include(x => x.Manufacturer)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var materialHardware in materialHardwareList)
                {
                    var materialHardwareDto = _mapper.Map<MaterialHardwareDto>(materialHardware);
                    materialHardwareDto.SupplierName = materialHardware.Supplier!=null?materialHardware.Supplier.SupplierName:"";
                    materialHardwareDtosList.Add(materialHardwareDto);

                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method End");
            return materialHardwareDtosList;
        }
        public async Task<List<MaterialHardwareDto>> GetAllMaterialHardwaresByMaterialId(Guid materialId)
        {
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method Start");
            List<MaterialHardwareDto> materialHardwareDtosList = new List<MaterialHardwareDto>();
            try
            {
                var materialHardwareList = await _genericRepo.Query().Where(x=>x.MaterialId== materialId)
                    .Include(x => x.Supplier)
                    .Include(x => x.Manufacturer)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var materialHardware in materialHardwareList)
                {
                    var materialHardwareDto = _mapper.Map<MaterialHardwareDto>(materialHardware);
                    materialHardwareDto.SupplierName = materialHardware.Supplier != null ? materialHardware.Supplier.SupplierName : "";
                    materialHardwareDtosList.Add(materialHardwareDto);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method End");
            return materialHardwareDtosList;
        }
        public async Task DeleteMaterialHardwareAsync(Guid Id)
        {
             _logger.LogInformation("MaterialHardwareService:DeleteMaterialHardwareAsync:Method End");
            try
            {
                var materialHardware = await _genericRepo.GetById(Id);
                await _genericRepo.Remove(materialHardware);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialHardwareService:DeleteMaterialHardwareAsync:Method End");
        }
    }
}
