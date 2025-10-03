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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialWarrantyService:IMaterialWarrantyService
    {
        
        private readonly IGenericRepository<MaterialWarranty> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialWarrantyService> _logger;
        public MaterialWarrantyService(ILogger<MaterialWarrantyService> logger, IGenericRepository<MaterialWarranty> genericRepo, IMapper mapper)
        {
            _logger = logger;
            _genericRepo = genericRepo;
            _mapper = mapper;
            
        }

        public async Task<MaterialWarrantyDto> AddWarrantyAsync(MaterialWarrantyDto materialWarrantyDto)
        {
            _logger.LogInformation("MaterialWarrantyService:AddWarrantyAsync:Method Start");
            try
            {
                var materialWarranty = _mapper.Map<MaterialWarranty>(materialWarrantyDto);
                materialWarranty.IsExtended = false;
                await _genericRepo.Add(materialWarranty);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:AddWarrantyAsync:Method End");
            return materialWarrantyDto;
        }

        public async Task DeleteWarrantyAsync(Guid id)
        {
            _logger.LogInformation("MaterialWarrantyService:DeleteWarrantyAsync:Method Start");
            try
            {
                var materialWarranty = await _genericRepo.GetById(id);
                await _genericRepo.Remove(materialWarranty);

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:DeleteWarrantyAsync:Method End");

        }

        public async Task<List<MaterialWarrantyDto>> GetAllMaterialWarranty()
        {
            _logger.LogInformation("MaterialWarrantyService:GetAllMaterialWarranty:Method Start");
           
            List<MaterialWarrantyDto> materialWarrantyDtoList = new List<MaterialWarrantyDto>();
            try
            {
                var materialWarrantyList = await _genericRepo.Query()
                    .Include(x => x.Material)                   
                    .ToListAsync().ConfigureAwait(false); ;
                foreach(var materialWarranty in materialWarrantyList)
                {
                    var materialWarrantyDto = _mapper.Map<MaterialWarrantyDto>(materialWarranty);
                    materialWarrantyDto.MaterialName = materialWarranty.Material.MaterialName;
                    materialWarrantyDto.MaterialCode = materialWarranty.Material.MaterialCode;                   
                    materialWarrantyDto.NoOfMonths = DateDiffInMonths(DateTime.Now, materialWarranty.WarrantyEndDate);
                    materialWarrantyDtoList.Add(materialWarrantyDto);
                }
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:GetAllMaterialWarranty:Method End");
            return materialWarrantyDtoList.OrderBy(x=>x.WarrantyEndDate).ToList();
            
        }
        public async Task<List<MaterialWarrantyDto>> GetMaterialWarrantyWithFilter(MaterialFilterDto materialFilterDto)
        {
            _logger.LogInformation("MaterialWarrantyService:GetMaterialWarrantyWithFilter:Method Start");

            List<MaterialWarrantyDto> materialWarrantyDtoList = new List<MaterialWarrantyDto>();
            try
            {
                var materialWarrantyList = await _genericRepo.Query()
                    .Where(x=>x.Material.WorkPackageId==materialFilterDto.WorkPackageId
                    && x.Material.System == materialFilterDto.System
                    && x.Material.IsRehabilitation == (materialFilterDto.KindOfMaterial == "Material" ? false : true))
                    .Include(x => x.Material)
                    .Include(x => x.Material.WorkPackage)
                    .Include(x => x.Material.Location)
                    .ToListAsync().ConfigureAwait(false); ;
                materialWarrantyDtoList.AddRange(_mapper.Map<List<MaterialWarrantyDto>>(materialWarrantyList));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:GetMaterialWarrantyWithFilter:Method End");
            return materialWarrantyDtoList.OrderBy(x => x.WarrantyEndDate).ToList();

        }
        public async Task<MaterialWarrantyDto> GetWarrantyByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialWarrantyService:GetWarrantyByIdAsync:Method Start");
            MaterialWarrantyDto materialWarrantyDto = new MaterialWarrantyDto();
            try
            {

                var materialWarranty = await _genericRepo.GetById(id);
                materialWarrantyDto = _mapper.Map<MaterialWarrantyDto>(materialWarranty);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:GetWarrantyByIdAsync:Method End");
            return materialWarrantyDto;
        }
        public async Task<MaterialWarrantyDto> GetWarrantyByMaterialIdAsync(Guid materialId)
        {
            _logger.LogInformation("MaterialWarrantyService:GetWarrantyByMaterialIdAsync:Method Start");
            MaterialWarrantyDto materialWarrantyDto = new MaterialWarrantyDto();
            try
            {

                var materialWarranty = await _genericRepo.Query()
                    .Where(x=>x.MaterialId==materialId && x.Material.IsRehabilitation==false)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                
                materialWarrantyDto = _mapper.Map<MaterialWarrantyDto>(materialWarranty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:GetWarrantyByMaterialIdAsync:Method End");
            return materialWarrantyDto;
        }

        public async Task<MaterialWarrantyDto> UpdateWarrantyAsync(MaterialWarrantyDto materialWarrantyDto)
        {
            _logger.LogInformation("MaterialWarrantyService:UpdateWarrantyAsync:Method Start");
            try
            {
                var existingWarranty = await _genericRepo.GetById(materialWarrantyDto.Id);
                if (existingWarranty != null)
                {
                    if (existingWarranty.WarrantyDocument != null)
                    {
                        existingWarranty.WarrantyDocument = materialWarrantyDto.WarrantyDocument;

                    }
                    existingWarranty.UpdatedDate = DateTime.Now;
                    existingWarranty.WarrantyStartDate = materialWarrantyDto.WarrantyStartDate;
                    existingWarranty.WarrantyEndDate = materialWarrantyDto.WarrantyEndDate;
                    existingWarranty.DLPStartDate = materialWarrantyDto.DLPStartDate;
                    existingWarranty.DLPEndDate = materialWarrantyDto.DLPEndDate;
                }
                var materialWarranty = _mapper.Map<MaterialWarranty>(materialWarrantyDto);
                await _genericRepo.Update(materialWarranty);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialWarrantyService:UpdateWarrantyAsync:Method End");
            return materialWarrantyDto;
        }
        private int DateDiffInMonths(DateTime startDate,DateTime endDate)
        {
            return (endDate.Year - startDate.Year)*12+endDate.Month-startDate.Month;
        }
    }
}
