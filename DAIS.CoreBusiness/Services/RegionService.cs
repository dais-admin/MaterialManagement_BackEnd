using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class RegionService:IRegionService
    {
        private IGenericRepository<Region> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionService> _logger;

        public RegionService(IGenericRepository<Region> genericRepo, IMapper mapper, ILogger<RegionService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RegionDto> GetRegionById(Guid id)
        {
            _logger.LogInformation("RegionService:GetRegionById:Method Start");
            RegionDto regionDto = new RegionDto();
            try
            {
                var region = await _genericRepo.GetById(id);
                regionDto = _mapper.Map<RegionDto>(region);
             
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:GetRegionById:Method End");
            return regionDto;
                
        }

        public async Task<RegionDto> AddRegion(RegionDto regionDto)
        {
            _logger.LogInformation("RegionService:AddRegion:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(regionDto.RegionName))
                {
                    regionDto.RegionName = regionDto.RegionName.ToUpper();
                }
                var region = _mapper.Map<Region>(regionDto);
                await _genericRepo.Add(region);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:AddRegion:Method End");
            return regionDto;
        }

        public async Task<RegionDto> UpdateRegion(RegionDto regionDto)
        { 

              _logger.LogInformation("RegionService:UpdateRegion:Method Start");

            try
            {
                var region = _mapper?.Map<Region>(regionDto);
                 await _genericRepo.Update(region);
               

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:UpdateRegion:Method End");
            return regionDto;

        }

        public async Task DeleteRegion(Guid id)
        {
            _logger.LogInformation("RegionService:DeleteRegion:Method Start");
            try
            {
                var region = await _genericRepo.GetById(id);
                await _genericRepo.Remove(region);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:DeleteRegion:Method End");
        }

       public async Task<List<RegionDto>> GetAllRegions()
        {
            _logger.LogInformation("RegionService:GetAllRegions:Method Start");
            List<RegionDto> regionDtoList = new List<RegionDto>();
            try
            {
                var regionsList = await _genericRepo.GetAll();

                foreach (var region in regionsList)
                {
                    
                    var regionDto=_mapper.Map<RegionDto>(region);
                    regionDtoList.Add(regionDto);

                }
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:GetAllRegions:Method End");
            return regionDtoList;

        }

        public RegionDto GetRegionIdByName(string name)
        {
            _logger.LogInformation("RegionService:GetRegionIdByName:Method Start");
            RegionDto regionDto = new RegionDto();
            try
            {
                var region = _genericRepo.Query()
                    .FirstOrDefault(x=>x.RegionName==name);
                regionDto = _mapper.Map<RegionDto>(region);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("RegionService:GetRegionIdByName:Method End");
            return regionDto;
        }
    }
}
