using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialHardwareService : IMaterialHardwareService
    {
        private IGenericRepository<MaterialHardware> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialHardwareService> _logger;
        private readonly IFileManagerService _fileManager;
        public MaterialHardwareService(IGenericRepository<MaterialHardware> genericRepo,IMapper mapper, ILogger<MaterialHardwareService> logger , IFileManagerService fileManager) 
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        
        }
        public  async Task<MaterialHardwareDto> AddMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto)
        {
            _logger.LogInformation("MaterialHardwareService:AddMaterialHardwareAsync:Method Start");
            try
            {
                var materialHardware = _mapper.Map<MaterialHardware>(materialHardwareDto);
                await _genericRepo.Add(materialHardware);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:AddMaterialHardwareAsync:Method End");
            return materialHardwareDto;
        }
        public async  Task<MaterialHardwareDto> UpdateMaterialHardwareAsync(MaterialHardwareDto materialHardwareDto)
        {
            _logger.LogInformation("MaterialHardwareService:UpdateMaterialHardwareAsync:Method Start");
            try
            {
                var existingHardwareDocument = await _genericRepo.GetById(materialHardwareDto.Id);
                if (existingHardwareDocument != null)
                {
                    if (!string.IsNullOrEmpty(materialHardwareDto.HardwareDocument))
                    {
                        existingHardwareDocument.HardwareDocument = materialHardwareDto.HardwareDocument;
                    }
                    existingHardwareDocument.UpdatedDate = DateTime.Now;
                    existingHardwareDocument.HarwareName = materialHardwareDto.HarwareName;
                    existingHardwareDocument.Chipset = materialHardwareDto.Chipset;
                    existingHardwareDocument.NetworkDetails = materialHardwareDto.NetworkDetails;
                    existingHardwareDocument.DateOfManufacturer = materialHardwareDto.DateOfManufacturer;
                    existingHardwareDocument.DiskDetails = materialHardwareDto.DiskDetails;
                    existingHardwareDocument.Remarks = materialHardwareDto.Remarks;
                    existingHardwareDocument.Quantity = materialHardwareDto.Quantity;
                }

                await _genericRepo.Update(existingHardwareDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:UpdateMaterialHardwareAsync:Method End");
            return materialHardwareDto;
        }
        public async Task<MaterialHardwareDto> GetMaterialHardwareByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialHardwareService:GetMaterialHardwareByIdAsync:Method Start");
            MaterialHardwareDto materialHardwareDto= new MaterialHardwareDto();
            try
            {
                var materialHardware = await _genericRepo.Query().Include(x=>x.Supplier).FirstOrDefaultAsync();
                materialHardware = _mapper.Map<MaterialHardware>(materialHardware);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetMaterialHardwareByIdAsync:Method End");
            return materialHardwareDto;
        }
        public async Task<List<MaterialHardwareDto>> GetAllMaterialHardware()
        {
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method Start");
            List<MaterialHardwareDto> materialHardwareDtosList = new List<MaterialHardwareDto>();
            try
            {
                var materialHardwareList = await _genericRepo.Query()
                    .Include(x => x.Supplier)
                    .Include(x => x.Manufacturer)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var materialHardware in materialHardwareList)
                {
                    var materialHardwareDto = _mapper.Map<MaterialHardwareDto>(materialHardware);
                    materialHardwareDto.SupplierName = materialHardware.Supplier!=null?materialHardware.Supplier.SupplierName:"";
                    materialHardwareDtosList.Add(materialHardwareDto);

                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method End");
            return materialHardwareDtosList;
        }
        public async Task<List<MaterialHardwareDto>> GetAllMaterialHardwaresByMaterialId(Guid materialId)
        {
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method Start");
            List<MaterialHardwareDto> materialHardwareDtosList = new List<MaterialHardwareDto>();
            try
            {
                var materialHardwareList = await _genericRepo.Query().Where(x=>x.MaterialId== materialId)
                    .Include(x => x.Supplier)
                    .Include(x => x.Manufacturer)
                    .ToListAsync().ConfigureAwait(false);
                foreach (var materialHardware in materialHardwareList)
                {
                    var materialHardwareDto = _mapper.Map<MaterialHardwareDto>(materialHardware);
                    materialHardwareDto.SupplierName = materialHardware.Supplier != null ? materialHardware.Supplier.SupplierName : "";
                    materialHardwareDtosList.Add(materialHardwareDto);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialHardwareService:GetAllMaterialHardware:Method End");
            return materialHardwareDtosList;
        }
        public async Task DeleteMaterialHardwareAsync(Guid id)
        {
            _logger.LogInformation("MaterialHardwareService:DeleteMaterialHardwareAsync:Method Start");

            try
            {
                var materialHardware = await _genericRepo.GetById(id);

                if (materialHardware == null)
                {
                    _logger.LogWarning($"MaterialHardware with id {id} not found.");
                    return;
                }

                // Delete associated files if exist
                if (!string.IsNullOrWhiteSpace(materialHardware.HardwareDocument))
                {
                    var files = materialHardware.HardwareDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);   // FileManager handles folder+file delete
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Remove DB entry
                await _genericRepo.Remove(materialHardware);

                _logger.LogInformation("MaterialHardwareService:DeleteMaterialHardwareAsync:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteMaterialHardwareAsync");
                throw;
            }
        }

    }
}
