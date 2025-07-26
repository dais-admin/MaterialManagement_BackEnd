using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class AgencyService : IAgencyService
    {
        private IGenericRepository<Agency> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AgencyService> _logger;
        
        public AgencyService(IGenericRepository<Agency> genericRepo,IMapper mapper,ILogger<AgencyService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            
        }
        public async Task<AgencyDto> AddAgency(AgencyDto agencyDto)
        {
            _logger.LogInformation("AgencyService:AddAgency:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(agencyDto.AgencyName))
                {
                    agencyDto.AgencyName = agencyDto.AgencyName.ToUpper();
                }
                var agency = _mapper.Map<Agency>(agencyDto);
                await _genericRepo.Add(agency);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("AgencyService:AddAgency:Method End");
            return agencyDto;
        }

        public async Task DeleteAgency(Guid id)
        {
            _logger.LogInformation("AgencyService:DeleteDivision:Method Start");
            try
            {
                var agency = await _genericRepo.GetById(id);
                await _genericRepo.Remove(agency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AgencyService:DeleteDivision:Method End");
        }

        public async Task<AgencyDto> GetAgency(Guid id)
        {
            _logger.LogInformation("AgencyService: GetAgency:Method Start");
            AgencyDto agencyDto = new AgencyDto();
            try
            {
                var agency = await _genericRepo.GetById(id);
                agencyDto = _mapper.Map<AgencyDto>(agency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AgencyService: GetAgency:Method End");
            return agencyDto;
        }

        public  AgencyDto GetAgencyIdByName(string name)
        {
            _logger.LogInformation("AgencyService:GetDivisionIdByName:Method Start");
            AgencyDto agencyDto = new AgencyDto();
            try
            {
                var division = _genericRepo.Query()
                    .FirstOrDefault(x => x.AgencyName == name);
                agencyDto = _mapper.Map<AgencyDto>(division);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AgencyService:GetDivisionIdByName:Method End");
            return agencyDto;
        }
    

        public async Task<List<AgencyDto>> GetAllAgency()
        {
            _logger.LogInformation("AgencyService:GetAllDivision:Method Start");
            List<AgencyDto> agencyDtoList = new List<AgencyDto>();
            try
            {
                var agencyList = await _genericRepo.GetAll();

                foreach (var agency in agencyList)
                {
                    var agencyDto = _mapper.Map<AgencyDto>(agency);
                    agencyDtoList.Add(agencyDto);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AgencyService:GetAllAgency:Method End");
            return agencyDtoList;
        }

        public async Task<AgencyDto> UpdateAgency(AgencyDto agencyDto)
        {
            _logger.LogInformation("AgencyService:UpdateAgency:Method Start");
            try
            {

                var agency = _mapper.Map<Agency>(agencyDto);
                await _genericRepo.Update(agency);
                return agencyDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("AgencyService:UpdateAgency:Method End");
            return agencyDto;
        }
    }
}
