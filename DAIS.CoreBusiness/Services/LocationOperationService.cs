using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class LocationOperationService : ILocationOperationService
    {
        private IGenericRepository<LocationOperation> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationOperationService> _logger;
        public LocationOperationService(IGenericRepository<LocationOperation> genericRepo, IMapper mapper, ILogger<LocationOperationService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<LocationOperationDto> GetLocationOperation(Guid id)
        {
            _logger.LogInformation("LocationOperationService:GetLocationOperation:Method Start");
            LocationOperationDto locationOperationDto = new LocationOperationDto();
            try
            {
               
                var locationOperation = await _genericRepo.Query()
                    .Where(x=>x.Id== id)
                    .Include(x => x.WorkPackage)
                    .Include(x => x.WorkPackage.Project)
                    .Include(x=>x.SubDivision)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                locationOperationDto = _mapper.Map<LocationOperationDto>(locationOperation);
                

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:GetLocationOperation:Method End");
            return  locationOperationDto;
            
            
        }

        public async  Task<LocationOperationDto> AddLocationOperation(LocationOperationDto locationOperationdto)
        {
            _logger.LogInformation("LocationOperationService:AddLocationOperation:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(locationOperationdto.LocationOperationName))
                {
                    locationOperationdto.LocationOperationName = locationOperationdto.LocationOperationName.ToUpper();
                }
                var locationoperation = _mapper.Map<LocationOperation>(locationOperationdto);
                await _genericRepo.Add(locationoperation);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:AddLocationOperation:Method End");
            return locationOperationdto;
        }

       public async Task DeleteLocationOperation(Guid id)
       {
            _logger.LogInformation("LocationOperationService:DeleteLocationOperation:Method Start");
            try
            {

                var LocationOperation = await _genericRepo.GetById(id);
                await _genericRepo.Remove(LocationOperation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:DeleteLocationOperation:Method End");

        }

      public async Task<LocationOperationDto>UpdateLocationOperation(LocationOperationDto locationOperationdto)
      {
            _logger.LogInformation("LocationOperationService:UpdateLocationOperation:Method Start");
            try
            {
                var locationOperation = _mapper.Map<LocationOperation>(locationOperationdto);
                await _genericRepo.Update(locationOperation);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:UpdateLocationOperation:Method End");
            return locationOperationdto;

      }

    public async Task<List<LocationOperationDto>> GetAllLocationOperation()
    {
            _logger.LogInformation("LocationOperationService:GetAllLocationOperation:Method Start");
            List<LocationOperationDto> locationOperationDtoList = new List<LocationOperationDto>();
            try
            {
                var locationOperationList = await _genericRepo.Query()
                    .Include(x=>x.WorkPackage)
                    .Include(x=>x.WorkPackage.Project)
                    .Include(x=>x.SubDivision)
                    .ToListAsync().ConfigureAwait(false);
                var locationDtoList = _mapper.Map<List<LocationOperationDto>>(locationOperationList);
                locationOperationDtoList.AddRange(locationDtoList);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:GetAllLocationOperation:Method End");
            return locationOperationDtoList;



      }
        public async Task<List<LocationOperationDto>> GetLocationsByWorkPackageId(Guid workPackageId)
        {
            _logger.LogInformation("LocationOperationService:GetLocationsByWorkPackageId:Method Start");
            List<LocationOperationDto> locationOperationDtoList = new List<LocationOperationDto>();
            try
            {
                var locations = await _genericRepo.Query()
                    .Where(x => x.WorkPackageId == workPackageId)
                    .Include(x => x.WorkPackage)
                    .Include(x=>x.SubDivision)
                    .ToListAsync().ConfigureAwait(false);
                locationOperationDtoList.AddRange(_mapper.Map<List<LocationOperationDto>>(locations));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("LocationOperationService:GetLocationsByWorkPackageId:Method End");
            return locationOperationDtoList;
        }

        public async Task<List<LocationOperationDto>> GetLocationsBySubDivisionId(Guid subDivisionId)
        {
            _logger.LogInformation("LocationOperationService:GetLocationsBySubDivisionId:Method Start");
            List<LocationOperationDto> locationOperationDtoList = new List<LocationOperationDto>();
            try
            {
                var locations = await _genericRepo.Query()
                    .Where(x => x.SubDivisionId == subDivisionId)
                    .Include(x => x.WorkPackage)
                    .Include(x => x.SubDivision)
                    .ToListAsync().ConfigureAwait(false);
                locationOperationDtoList.AddRange(_mapper.Map<List<LocationOperationDto>>(locations));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("LocationOperationService:GetLocationsBySubDivisionId:Method End");
            return locationOperationDtoList;
        }

        public LocationOperationDto GetLocationIdByName(string name,string system)
        {
            _logger.LogInformation("LocationOperationService:GetLocationIdByName:Method Start");
            LocationOperationDto locationOperationDto = new LocationOperationDto();
            try
            {

                var locationOperation =  _genericRepo.Query()
                    .FirstOrDefault(x=>x.LocationOperationName.ToLower() == name.ToLower()
                    && x.System==system);
                locationOperationDto = _mapper.Map<LocationOperationDto>(locationOperation);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("LocationOperationService:GetLocationIdByName:Method End");
            return locationOperationDto;

        }
    }
}
