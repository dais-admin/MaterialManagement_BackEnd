using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialTypeService : IMaterialTypeService
    {
        private readonly IGenericRepository<MaterialType> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialTypeService> _logger;
        public MaterialTypeService(IGenericRepository<MaterialType> genericRepo,
            IMapper mapper, ILogger<MaterialTypeService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<MaterialTypeDto> AddMaterialType(MaterialTypeDto materialTypeDto)
        {
            _logger.LogInformation("MaterialTypeService:AddMaterialType:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(materialTypeDto.TypeName))
                {
                    materialTypeDto.TypeName = materialTypeDto.TypeName.ToUpper();
                }
                var materialType = _mapper.Map<MaterialType>(materialTypeDto);
                await _genericRepo.Add(materialType).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to add the {nameof(MaterialType)}", ex);
                throw;
            }
            _logger.LogInformation("MaterialTypeService:AddMaterialType:Method End");
            return materialTypeDto;
        }

        public async Task DeleteMaterialType(Guid id)
        {
            _logger.LogInformation("MaterialTypeService:DeleteMaterialType:Method Start");
            try
            {
                var materialType = await _genericRepo.GetById(id);
                await _genericRepo.Remove(materialType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialTypeService:DeleteMaterialType:Method End");
        }

        public async Task<List<MaterialTypeDto>> GetAllMaterialTypes()
        {
            _logger.LogInformation("MaterialTypeService:GetAllMaterialTypes:Method Start");
            List<MaterialTypeDto> materialTypeDtoList = new List<MaterialTypeDto>();
            try
            {
                var materialTypeList = await _genericRepo.Query().Include(x => x.Project).ToListAsync().ConfigureAwait(false);
                foreach (var materialType in materialTypeList)
                {
                    materialTypeDtoList.Add(_mapper.Map<MaterialTypeDto>(materialType));
                }
               
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialTypeService:GetAllMaterialTypes:Method End");
            return materialTypeDtoList;
        }

        public async Task<MaterialTypeDto> GetMaterialTypeById(Guid id)
        {
            _logger.LogInformation("MaterialTypeService:GetMaterialTypeById:Method Start");

            var materialTypeDto = new MaterialTypeDto();
            try
            {
                var materialType = await _genericRepo.Query().Include(x => x.Project).FirstOrDefaultAsync(x=>x.Id==id).ConfigureAwait(false);
               // var materialType = await _genericRepo.Query().Include(x=>x.Project).Where(x=>x.Id==id).FirstOrDefault().;
                materialTypeDto =_mapper.Map<MaterialTypeDto>(materialType);

                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialTypeService:GetMaterialTypeById:Method End");
            return materialTypeDto;
        }

        public MaterialTypeDto GetMaterialTypeIdByName(string name)
        {
            _logger.LogInformation("MaterialTypeService:GetMaterialTypeIdByName:Method Start");

            var materialTypeDto = new MaterialTypeDto();
            try
            {
                var materialType = _genericRepo.Query()
                    .FirstOrDefault(x => x.TypeName.ToLower() == name.ToLower());
               materialTypeDto = _mapper.Map<MaterialTypeDto>(materialType);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialTypeService:GetMaterialTypeById:Method End");
            return materialTypeDto;
        }

        public async Task<MaterialTypeDto> UpdateMaterialType(MaterialTypeDto materialTypeDto)
        {
            _logger.LogInformation("MaterialTypeService:UpdateMaterialType:Method Start");
            try
            {
                var materialType = _mapper.Map<MaterialType>(materialTypeDto);
                await _genericRepo.Update(materialType).ConfigureAwait(false);
               
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialTypeService:UpdateMaterialType:Method End");
            return materialTypeDto;
        }
    }
}
