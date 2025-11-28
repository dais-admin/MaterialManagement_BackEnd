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
        private readonly IFileManagerService _fileManager;
        public MaterialSoftwareService(IGenericRepository<MaterialSoftware> genericRepo, IMapper mapper, ILogger<MaterialSoftwareService> logger , IFileManagerService fileManager) 
        { 
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        
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

        public async Task DeleteMaterialSoftwareAsync(Guid id)
        {
            _logger.LogInformation("MaterialSoftwareService:DeleteMaterialSoftwareAsync:Method Start");

            try
            {
                var materialSoftware = await _genericRepo.GetById(id);

                if (materialSoftware == null)
                {
                    _logger.LogWarning($"MaterialSoftware with id {id} not found.");
                    return;
                }

                // Delete attached files/folders
                if (!string.IsNullOrWhiteSpace(materialSoftware.SoftwareDocument))
                {
                    var files = materialSoftware.SoftwareDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);     // FileManager deletes file + folder
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Remove record from DB
                await _genericRepo.Remove(materialSoftware);

                _logger.LogInformation("MaterialSoftwareService:DeleteMaterialSoftwareAsync:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteMaterialSoftwareAsync");
                throw;
            }
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
                var materialSoftware = await _genericRepo.Query().Include(x => x.Supplier).FirstOrDefaultAsync();
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
                    if (!string.IsNullOrEmpty(materialSoftwareDto.SoftwareDocument))
                    {
                        existingSoftwareDocument.SoftwareDocument = materialSoftwareDto.SoftwareDocument;
                    }

                    existingSoftwareDocument .Quantity = materialSoftwareDto.Quantity;
                    existingSoftwareDocument.StartDate = materialSoftwareDto.StartDate;
                    existingSoftwareDocument.EndDate = materialSoftwareDto.EndDate;
                    existingSoftwareDocument.Remarks = materialSoftwareDto.Remarks;
                }
               
                await _genericRepo.Update(existingSoftwareDocument);
               
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
