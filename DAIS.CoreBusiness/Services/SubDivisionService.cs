using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class SubDivisionService : ISubDivisionService
    {
        private IGenericRepository<SubDivision> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<SubDivisionService> _logger;
        public SubDivisionService(IGenericRepository<SubDivision> genericRepo, IMapper mapper, ILogger<SubDivisionService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<SubDivisionDto> AddSubDivision(SubDivisionDto subDivisionDto)
        {
            _logger.LogInformation("SubDivisionService:AddSubDivision:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(subDivisionDto.SubDivisionName))
                {
                    subDivisionDto.SubDivisionName = subDivisionDto.SubDivisionName.ToUpper();
                }
                var subDivision = _mapper.Map<SubDivision>(subDivisionDto);
                await _genericRepo.Add(subDivision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SubDivisionService:AddSubDivision:Method End");
            return subDivisionDto;

        }

        public async Task DeleteSubDivision(Guid id)
        {
            _logger.LogInformation("SubDivisionService:DeleteSubDivision:Method Start");
            try
            {
                var subDivision = await _genericRepo.GetById(id);
                await _genericRepo.Remove(subDivision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SubDivisionService:DeleteSubDivision:Method End");
        }

        public async Task<List<SubDivisionDto>> GetAllSubDivision()
        {
            _logger.LogInformation("SubDivisionService:GetAllSubDivision:Method Start");

            List<SubDivisionDto> subDivisionDtoList = new List<SubDivisionDto>();
            try
            {

                var subDivisionList = await _genericRepo.Query()
                 .Include(x => x.Division).ToListAsync().ConfigureAwait(false);
                subDivisionDtoList.AddRange(_mapper.Map<List<SubDivisionDto>>(subDivisionList));


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("SubDivisionService:GetAllSubDivision:Method End");
            return subDivisionDtoList;
        }
        public async Task<List<SubDivisionDto>> GetAllSubDivisionsByDivision(Guid divisionId)
        {
            _logger.LogInformation("SubDivisionService:GetAllSubDivisionsByDivision:Method Start");

            List<SubDivisionDto> subDivisionDtoList = new List<SubDivisionDto>();
            try
            {

                var subDivisionList = await _genericRepo.Query()
                 .Where(x=>x.DivisionId==divisionId)
                 .Include(x => x.Division).ToListAsync().ConfigureAwait(false);
                subDivisionDtoList.AddRange(_mapper.Map<List<SubDivisionDto>>(subDivisionList));


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("SubDivisionService:GetAllSubDivisionsByDivision:Method End");
            return subDivisionDtoList;
        }
        public async Task<SubDivisionDto> GetSubDivision(Guid id)
        {
            _logger.LogInformation("SubDivisionService:GetSubDivision:Method Start");
            SubDivisionDto subDivisionDto = new SubDivisionDto();
            try
            {
                var subDivision = await _genericRepo.Query()
                 .Include(x => x.Division).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

                subDivisionDto = _mapper.Map<SubDivisionDto>(subDivision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SubDivisionService:GetSubDivision:Method Start");
            return subDivisionDto;
        }

        public SubDivisionDto GetSubDivisionIdByName(string name)
        {
            _logger.LogInformation("SubDivisionService:GetSubDivisionIdByName:Method Start");
            SubDivisionDto subDivisionDto = new SubDivisionDto();
            try
            {
                var subDivision = _genericRepo.Query()
                 .FirstOrDefault(x => x.SubDivisionName.ToLower() == name.ToLower());
                subDivisionDto = _mapper.Map<SubDivisionDto>(subDivision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SubDivisionService:GetSubDivisionIdByName:Method End");
            return subDivisionDto;
        }

        public async Task<SubDivisionDto> UpdateSubDivision(SubDivisionDto subDivisionDto)
        {
            _logger.LogInformation("SubDivisionService:UpdateSubDivision:Method Start");
            try
            {

                var subdivision = _mapper.Map<SubDivision>(subDivisionDto);

                await _genericRepo.Update(subdivision);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("SubDivisionService:UpdateSubDivision:Method End");
            return subDivisionDto;
        }
    }
}
