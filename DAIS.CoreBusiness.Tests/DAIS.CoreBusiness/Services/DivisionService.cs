using AutoMapper;
using DAIS.CoreBusiness.Dtos;
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
    public class DivisionService : IDivisionService
    {
        private IGenericRepository<Division> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DivisionService> _logger;
        
        public DivisionService(IGenericRepository<Division> genericRepo, IMapper mapper, ILogger<DivisionService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            
        }
        public async Task<DivisionDto> AddDivision(DivisionDto divisiondto)
        {
            _logger.LogInformation("DivisionService:AddDivision:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(divisiondto.DivisionName))
                {
                    divisiondto.DivisionName = divisiondto.DivisionName.ToUpper();
                }
                var division = _mapper.Map<Division>(divisiondto);
                await _genericRepo.Add(division);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("DivisionService:AddDivision:Method End");
            return divisiondto;
        }

        public async Task DeleteDivision(Guid id)
        {
            _logger.LogInformation("DivisionService:DeleteDivision:Method Start");
            try
            {
                var division = await _genericRepo.GetById(id);
                await _genericRepo.Remove(division);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:DeleteDivision:Method End");
        }

        public async Task <List<DivisionDto>> GetAllDivision()
        {
            _logger.LogInformation("DivisionService:GetAllDivision:Method Start");
            List<DivisionDto> divisionDtoList = new List<DivisionDto>();
            try
            {
                var divisionList = await _genericRepo.Query()                   
                    .ToListAsync().ConfigureAwait(false);
                divisionDtoList.AddRange(_mapper.Map<List<DivisionDto>>(divisionList));
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:GetAllDivision:Method End");
            return divisionDtoList;

        }

        public async Task<DivisionDto> GetDivision(Guid id)
        {
            _logger.LogInformation("DivisionService:GetDivision:Method Start");
            DivisionDto divisionDto = new DivisionDto();
            try
            {
                var division = await _genericRepo.Query()
                    .FirstOrDefaultAsync(x=>x.Id==id);
                divisionDto=_mapper.Map<DivisionDto>(division);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:GetDivision:Method End");
            return divisionDto;
           
            
        }
        public async Task<List<DivisionDto>> GetDivisionsByLocation(Guid locationId)
        {
            _logger.LogInformation("DivisionService:GetDivision:Method Start");
            List<DivisionDto> divisionDtoList = new List<DivisionDto>();
            try
            {
                var divisionlist = await _genericRepo.Query()
                    .ToListAsync();
                divisionDtoList.AddRange(_mapper.Map<List<DivisionDto>>(divisionlist));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:GetDivision:Method End");
            return divisionDtoList;
        }
        public async Task<DivisionDto> UpdateDivision(DivisionDto divisiondto)
        {
            _logger.LogInformation("DivisionService:UpdateDivision:Method Start");
            try
            {
                
                 var division =_mapper.Map<Division>(divisiondto);
                 await _genericRepo.Update(division);
                 return divisiondto;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:UpdateDivision:Method End");
            return divisiondto ;
        }

        public DivisionDto GetDivisionIdByName(string name)
        {
            _logger.LogInformation("DivisionService:GetDivisionIdByName:Method Start");
            DivisionDto divisionDto = new DivisionDto();
            try
            {
                var division = _genericRepo.Query()
                    .FirstOrDefault(x=>x.DivisionName.ToLower()==name.ToLower()
                    ) ;
                divisionDto = _mapper.Map<DivisionDto>(division);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("DivisionService:GetDivisionIdByName:Method End");
            return divisionDto;
        }
    }
}
