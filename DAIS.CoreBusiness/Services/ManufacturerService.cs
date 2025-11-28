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
    public class ManufacturerService:IManufacturerService
    {
        private IGenericRepository<Manufacturer> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ManufacturerService> _logger;
        private readonly IFileManagerService _fileManager;
        public ManufacturerService(IGenericRepository<Manufacturer> genericRepo, IMapper mapper, ILogger<ManufacturerService> logger , IFileManagerService fileManager)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        }

        public async Task<ManufacturerDto> GetManufacturer(Guid id)
        {
            _logger.LogInformation("ManufacturerService:GetManufacturer:Method Start");
            ManufacturerDto manufacturerDto = new ManufacturerDto();
            try
            {
                var manufacturer = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);              
                manufacturerDto =_mapper.Map<ManufacturerDto>(manufacturer);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ManufacturerService:GetManufacturer:Method End");
            return manufacturerDto;
        }

        public  async Task<ManufacturerDto> AddManufacturer(ManufacturerDto manufacturerDto)
        {
            _logger.LogInformation("ManufacturerService:AddManufacturer:Method Start");

            try
            {
                if (!string.IsNullOrEmpty(manufacturerDto.ManufacturerName))
                {
                    manufacturerDto.ManufacturerName = manufacturerDto.ManufacturerName.ToUpper();
                }
                var manufacturer = _mapper.Map<Manufacturer>(manufacturerDto);
                await _genericRepo.Add(manufacturer);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ManufacturerService:AddManufacturer:Method End");
            return manufacturerDto;
        }

        public async Task<ManufacturerDto> UpdateManufactuter(ManufacturerDto manufacturerDto)
        {
            _logger.LogInformation("ManufacturerService:UpdateManufactuter:Method  Start");
            try
            {
                var existingManufacturer = await _genericRepo.GetById(manufacturerDto.Id);
                if (existingManufacturer != null)
                {
                    if (!string.IsNullOrEmpty(manufacturerDto.ManufacturerDocument))
                    {
                        existingManufacturer.ManufacturerDocument = manufacturerDto.ManufacturerDocument;
                    }
                    existingManufacturer.UpdatedDate = DateTime.Now;
                    existingManufacturer.ManufacturerName = manufacturerDto.ManufacturerName;
                    existingManufacturer.ManufacturerAddress = manufacturerDto.ManufacturerAddress;
                    existingManufacturer.ProductsDetails = manufacturerDto.ProductsDetails;
                    existingManufacturer.ImportantDetails = manufacturerDto.ImportantDetails;
                    existingManufacturer.ContactNo = manufacturerDto.ContactNo;
                    existingManufacturer.ContactEmail = manufacturerDto.ContactEmail;
                }

                await _genericRepo.Update(existingManufacturer);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ManufacturerService:UpdateManufactuter:Method  End");
            return manufacturerDto;
        }


        public async Task DeleteManufacturer(Guid id)
        {
            _logger.LogInformation("ManufacturerService:DeleteManufacturer:Method Start");

            try
            {
                var manufacturer = await _genericRepo.GetById(id);

                if (manufacturer == null)
                {
                    _logger.LogWarning($"Manufacturer with id {id} not found.");
                    return;
                }

                // Assuming this field contains the file paths
                if (!string.IsNullOrWhiteSpace(manufacturer.ManufacturerDocument))
                {
                    var files = manufacturer.ManufacturerDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);  // Use your FileManagerService
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                // Delete database record
                await _genericRepo.Remove(manufacturer);

                _logger.LogInformation("ManufacturerService:DeleteManufacturer:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteManufacturer");
                throw;
            }
        }


        public async  Task<List<ManufacturerDto>> GetAllManufacturer()
        {
            _logger.LogInformation("ManufacturerService: GetAllManufacturer:Method Start");
            List<ManufacturerDto> manufacturerDtoList = new List<ManufacturerDto>();
            try
            {
                var manufacturerList = await _genericRepo.Query()
                 .Include(x => x.MaterialType)
                 .Include(x => x.Category).ToListAsync().ConfigureAwait(false);
                manufacturerDtoList.AddRange(_mapper.Map<List<ManufacturerDto>>(manufacturerList));              
            }
            catch (Exception ex )
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ManufacturerService:GetAllManufacturer:Method End ");
            return manufacturerDtoList;

        }

        public ManufacturerDto GetManufacturerIdByName(string name)
        {
            _logger.LogInformation("ManufacturerService:GetManufacturerIdByName:Method Start");
            ManufacturerDto manufacturerDto = new ManufacturerDto();
            try
            {
                var manufacturer = _genericRepo.Query()
                 .FirstOrDefault(x => x.ManufacturerName.ToLower() == name.ToLower());
                manufacturerDto = _mapper.Map<ManufacturerDto>(manufacturer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("ManufacturerService:GetManufacturerIdByName:Method End");
            return manufacturerDto;
        }
    }

}
