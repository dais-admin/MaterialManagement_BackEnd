using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using DAIS.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class InventoryService:IInventoryService
    {
        private readonly IGenericRepository<Material> _repoMaterial;
        private readonly IGenericRepository<MaterialWarranty> _repoMaterialWarranty;
        private readonly MaterialWarrantyService _materialWarrantyService;
        private readonly ILogger<InventoryService> _logger;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        public InventoryService(IGenericRepository<Material> repoMaterial, IGenericRepository<MaterialWarranty> repoMaterialWarranty,
           MaterialServiceInfrastructure materialServiceInfrastructure, ILogger<InventoryService> logger)
        {
            _repoMaterial= repoMaterial;
            _repoMaterialWarranty = repoMaterialWarranty;
            _materialServiceInfrastructure= materialServiceInfrastructure;
            _logger = logger;
        }

        public async Task<MaterialInventoryDto> GetMaterialInventoryByCodeAsync(string materialCode)
        {
            _logger.LogInformation("MaterialService:GetMaterialInventoryByCode:Method Start");
            MaterialInventoryDto materialInventoryDto = new MaterialInventoryDto();
            try
            {
                var material = await _repoMaterial.Query().Where(x => x.MaterialCode == materialCode)
                    
                    .Include(x => x.Category)
                    .ThenInclude(x => x.MaterialType)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .ThenInclude(x => x.SubDivision)
                    .ThenInclude(x => x.Division)
                    .Include(x => x.Supplier) 
                    .Include(x=>x.WorkPackage)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                 var materialDto= _materialServiceInfrastructure.Mapper.Map<MaterialDto>(material);
                //    materialDto.CategoryName = material.Category.CategoryName;
                 materialDto.MaterialType = _materialServiceInfrastructure.Mapper.Map<MaterialTypeDto>( material.Category.MaterialType);
                //materialDto.ManufacturerName = material.Manufacturer.ManufacturerName;
                var materialWarranty = await _repoMaterialWarranty.Query().Where(x => x.MaterialId == materialDto.Id)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                var materialWarrantyDto = _materialServiceInfrastructure.Mapper.Map<MaterialWarrantyDto>(materialWarranty);
                materialInventoryDto.Material = materialDto;
                materialInventoryDto.MaterialWarranty = materialWarrantyDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialService:GetMaterialInventoryByCode:Method End");
            return materialInventoryDto;
        }
        public async Task<MaterialDto> UpdateMaterialAsync(MaterialDto materialDto)
        {
            _logger.LogInformation("MaterialService:UpdateMaterialAsync:Method Start");
            try
            {
                var existingMaterial = await _materialServiceInfrastructure.GenericRepository.GetById(materialDto.Id);
                existingMaterial.MaterialName = materialDto.MaterialName;
                existingMaterial.CategoryId = materialDto.CategoryId;
                existingMaterial.TypeId= materialDto.TypeId;               
                existingMaterial.RegionId = materialDto.RegionId;
                existingMaterial.LocationId = materialDto.LocationId;
                existingMaterial.WorkPackageId = materialDto.WorkPackageId;
                existingMaterial.ModelNumber = materialDto.ModelNumber;
                existingMaterial.TagNumber = materialDto.TagNumber;
                existingMaterial.ManufacturerId = materialDto.ManufacturerId;
                existingMaterial.SupplierId = materialDto.SupplierId;
                existingMaterial.PurchaseDate = materialDto.PurchaseDate;
                existingMaterial.YearOfInstallation = materialDto.YearOfInstallation;
                existingMaterial.DesignLifeDate = materialDto.DesignLifeDate;
                existingMaterial.EndPeriodLifeDate = materialDto.EndPeriodLifeDate;
                existingMaterial.IsRehabilitation = materialDto.IsRehabilitation;
                existingMaterial.LocationRFId= materialDto.LocationRFId;
                existingMaterial.CommissioningDate= materialDto.CommissioningDate;
                existingMaterial.MaterialStatus = (MaterialStatus)Enum.Parse(typeof(MaterialStatus), materialDto.MaterialStatus);

                 //var material = _materialServiceInfrastructure.Mapper.Map<MaterialDto, Material>(materialDto);
                 await _materialServiceInfrastructure.GenericRepository.Update(existingMaterial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialService:UpdateMaterialAsync:Method End");
            return materialDto;
        }
    }
}
