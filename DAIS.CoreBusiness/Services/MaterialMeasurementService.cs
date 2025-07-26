using AutoMapper;
using Castle.Core.Logging;
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
    public class MaterialMeasurementService : IMaterialMeasurementService
    {
        private IGenericRepository<MaterialMeasurement> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialMeasurementService> _logger;
        public MaterialMeasurementService(IGenericRepository<MaterialMeasurement> genericRepo, IMapper mapper, ILogger<MaterialMeasurementService> logger) 
        { 
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<MaterialMeasuremetDto>AddMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto)
        {
            _logger.LogInformation("MaterialMeasurementService:AddMeasurement:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(materialMeasuremetDto.MeasurementName))
                {
                    materialMeasuremetDto.MeasurementName = materialMeasuremetDto.MeasurementName.ToUpper();
                }
                var measurement = _mapper.Map<MaterialMeasurement>(materialMeasuremetDto);
                await _genericRepo.Add(measurement);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialMeasurementService:AddMeasurement:Method End");
            return materialMeasuremetDto;

        }

        public async Task DeleteMeasurement(Guid id)
        {
            _logger.LogInformation("MaterialMeasurementService:DeleteMeasurement:Method Start");
            try
            {
                var measurement = await _genericRepo.GetById(id);
                await _genericRepo.Remove(measurement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMeasurementService:DeleteMeasurement:Method End");
        }


        public async Task<List<MaterialMeasuremetDto>> GetAllMaterialMeasurement()
        {
            _logger.LogInformation("MaterialMeasurementService:GetAllMaterialMeasurement:Method Start");
            List<MaterialMeasuremetDto> materialmeasurementDtoList = new List<MaterialMeasuremetDto>();
            try
            {
                var measurementList = await _genericRepo.GetAll();

                foreach (var measurement in measurementList)
                {
                    var measuremetDto = _mapper.Map<MaterialMeasuremetDto>(measurement);
                    materialmeasurementDtoList.Add(measuremetDto);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMeasurementService:GetAllMaterialMeasurement:Method End");
            return materialmeasurementDtoList;

        }
        public async Task<MaterialMeasuremetDto> GetMaterialMeasurement(Guid id)
        {
            _logger.LogInformation("MaterialMeasurementService:GetMaterialMeasurement:Method Start");
            MaterialMeasuremetDto materialMeasuremetDto  = new MaterialMeasuremetDto();
            try
            {
                var measurement = await _genericRepo.GetById(id);
                materialMeasuremetDto = _mapper.Map<MaterialMeasuremetDto>(measurement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMeasurementService:GetMaterialMeasurement:Method End");
            return materialMeasuremetDto;

        }

        public async Task<MaterialMeasuremetDto> UpdateMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto)
        {
            _logger.LogInformation("MaterialMeasurementService:UpdateMeasurement:Method Start");
            try
            {

                var measurement = _mapper.Map<MaterialMeasurement>(materialMeasuremetDto);
                await _genericRepo.Update(measurement);
                return materialMeasuremetDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMeasurementService:UpdateMeasurement:Method End");
            return materialMeasuremetDto;
        }

        public MaterialMeasuremetDto GetMeasuremetByName(string name)
        {
            _logger.LogInformation("MaterialMeasurementService:GetMeasuremetByName:Method Start");
            MaterialMeasuremetDto materialMeasuremetDto = new MaterialMeasuremetDto();
            try
            {
                var measurement = _genericRepo.Query()
                    .FirstOrDefault(x=>x.MeasurementName==name);
                materialMeasuremetDto = _mapper.Map<MaterialMeasuremetDto>(measurement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialMeasurementService:GetMeasuremetByName:Method End");
            return materialMeasuremetDto;
        }
    }
}
