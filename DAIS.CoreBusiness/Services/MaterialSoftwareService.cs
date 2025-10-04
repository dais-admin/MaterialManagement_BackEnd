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
    public class MaterialSoftwareService:IMaterialSoftwareService
    {
        private IGenericRepository<MaterialSoftware> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialSoftwareService> _logger;
        public MaterialSoftwareService(IGenericRepository<MaterialSoftware> genericRepo, IMapper mapper, ILogger<MaterialSoftwareService> logger) 
        { 
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        
        }

        public async Task<MaterialSoftwareDto> AddMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto)
        {
            _logger.LogInformation("MaterialSoftwareService:AddMaterialSoftwareAsync:Method Start");
            try
            {
                var materialSoftware = _mapper.Map<MaterialSoftware>(materialSoftwareDto);
                await _genericRepo.Add(materialSoftware);
               
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialSoftwareService:AddMaterialSoftwareAsync:Method End");
            return materialSoftwareDto;
        }

        public async Task DeleteMaterialSoftwareAsync(Guid Id)
        {
            _logger.LogInformation("MaterialHardwareService: DeleteMaterialSoftwareAsync:Method End");
            try
            {
                var materialSoftware = await _genericRepo.GetById(Id);
                
                await _genericRepo.Remove(materialSoftware);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("MaterialSoftwareService:DeleteMaterialHardwareAsync:Method End");
        }

        public async Task<List<MaterialSoftwareDto>> GetAllMaterialSoftware()
        {
            _logger.LogInformation("MaterialSoftwareService:GetAllMaterialSoftware:Method Start");
            List<MaterialSoftwareDto> materialSoftwareDtosList = new List<MaterialSoftwareDto>();
           try
           {
               var materialSoftwareList = await _genericRepo.GetAll();
               foreach (var materialSoftware in materialSoftwareList)
               {
                   var materialSoftwareDto = _mapper.Map<MaterialSoftwareDto>(materialSoftwareList);
                  materialSoftwareDtosList.Add(materialSoftwareDto);

               }
           }
           catch (Exception ex)
            {
               _logger.LogError(ex.Message, ex);
                 throw ex;
            }
            _logger.LogInformation("MaterialSoftwareService: GetAllMaterialSoftware:Method End");
            return materialSoftwareDtosList;
        }
        public async Task<List<MaterialSoftwareDto>> GetSoftwareListByMaterialIdAsync(Guid materialId)
        {
            _logger.LogInformation("MaterialSoftwareService:GetSoftwareListByMaterialIdAsync:Method Start");
            List<MaterialSoftwareDto> materialSoftwareDtosList = new List<MaterialSoftwareDto>();
            try
            {
                var materialSoftwareList = await _genericRepo.Query().Where(x=>x.MaterialId==materialId)
                    .Include(x => x.Supplier)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var materialSoftware in materialSoftwareList)
                {
                    var materialSoftwareDto = _mapper.Map<MaterialSoftwareDto>(materialSoftware);
                    materialSoftwareDto.SupplierName = materialSoftware.Supplier!=null? materialSoftware.Supplier.SupplierName:"";
                    materialSoftwareDtosList.Add(materialSoftwareDto);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialSoftwareService: GetSoftwareListByMaterialIdAsync:Method End");
            return materialSoftwareDtosList;
        }

        public async Task<MaterialSoftwareDto> GetMaterialSoftwareByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialSoftwareService:GetMaterialSoftwareByIdAsync:Method Start");
            MaterialSoftwareDto materialSoftwareDto = new MaterialSoftwareDto();
            try
            {
                var materialSoftware = await _genericRepo.GetById(id);
                materialSoftwareDto = _mapper.Map<MaterialSoftwareDto>(materialSoftware);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialSoftwareService:GetMaterialSoftwareByIdAsync:Method End");
            return materialSoftwareDto;
        }

        public async Task<MaterialSoftwareDto> UpdateMaterialSoftwareAsync(MaterialSoftwareDto materialSoftwareDto)
        {
            _logger.LogInformation("MaterialSoftwareService: UpdateMaterialSoftwareAsync:Method Start");
            try
            {
                var existingSoftwareDocument = await _genericRepo.GetById(materialSoftwareDto.Id);
               if(existingSoftwareDocument != null)
                {
                    if (existingSoftwareDocument.SoftwareDocument != null)
                    {
                        existingSoftwareDocument.SoftwareDocument = materialSoftwareDto.SoftwareDocument;
                    }

                    existingSoftwareDocument .Quantity = materialSoftwareDto.Quantity;
                    existingSoftwareDocument.StartDate = materialSoftwareDto.StartDate;
                    existingSoftwareDocument.EndDate = materialSoftwareDto.EndDate;
                    existingSoftwareDocument.Remarks = materialSoftwareDto.Remarks;
                }
                var materialSoftware=_mapper.Map<MaterialSoftware>(materialSoftwareDto);
                await _genericRepo.Update(materialSoftware);
               
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialSoftwareService:AddMaterialSoftwareAsync:Method End");
            return materialSoftwareDto;


        }
    }
}
