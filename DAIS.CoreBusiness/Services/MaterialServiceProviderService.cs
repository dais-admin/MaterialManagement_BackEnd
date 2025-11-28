using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialServiceProviderService : IMaterialServiceProviderService
    {
        private IGenericRepository<MaterialServiceProvider> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialServiceProviderService> _logger;
        private readonly string _rootFolder = string.Empty;
        private readonly IFileManagerService _fileManager;

        public MaterialServiceProviderService(IGenericRepository<MaterialServiceProvider> genericRepo,  IMapper mapper, ILogger<MaterialServiceProviderService> logger, IFileManagerService fileManager)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            _fileManager = fileManager;
        }
        public  async Task<MaterialServiceProviderDto> AddServiceProviderAsync(MaterialServiceProviderDto serviceProviderDto)
        {
            _logger.LogInformation("MaterialServiceProvider:AddServiceProviderAsync:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(serviceProviderDto.ServiceProviderName))
                {
                    serviceProviderDto.ServiceProviderName = serviceProviderDto.ServiceProviderName.ToUpper();
                }
                var serviceprovider = _mapper.Map<MaterialServiceProvider>(serviceProviderDto);
                await _genericRepo.Add(serviceprovider);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialServiceProvider:AddServiceProviderAsync:Method End");
            return serviceProviderDto;
        }

        public async Task DeleteServiceProviderAsync(Guid id)
        {
            _logger.LogInformation("MaterialServiceProvider:DeleteServiceProviderAsync:Method Start");
            try
            {
                var serviceProvider = await _genericRepo.GetById(id);
                if (serviceProvider == null)
                {
                    _logger.LogWarning($"ServiceProvider with id {id} not found.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(serviceProvider.ServiceProviderDocument))
                {
                    var files = serviceProvider.ServiceProviderDocument
                                .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filePath in files)
                    {
                        try
                        {
                            _fileManager.Delete(filePath);  // Let FileManagerService handle path
                            _logger.LogInformation($"Deleted file: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Failed to delete file: {filePath}");
                        }
                    }
                }

                await _genericRepo.Remove(serviceProvider);

                _logger.LogInformation("MaterialServiceProvider:DeleteServiceProviderAsync:Method End");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceProviderAsync");
                throw;
            }
        }


        public async Task<PagedResult<MaterialServiceProviderDto>> GetAllServiceProviderAsync(
     int pageNumber, int pageSize)
        {
            _logger.LogInformation("MaterialServiceProvider:GetAllServiceProviderAsync:Method Start");

            try
            {
                var query = _genericRepo.Query()
                    .Include(x => x.Contractor)
                    .Include(x => x.Manufacturer)
                    .AsQueryable();

                var totalRecords = await query.CountAsync();

                var pagedData = await query
                    .OrderBy(x => x.ServiceProviderName) // change based on your field
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtoData = _mapper.Map<List<MaterialServiceProviderDto>>(pagedData);

                return new PagedResult<MaterialServiceProviderDto>
                {
                    TotalCount = totalRecords,
                    Data = dtoData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }


        public async Task<MaterialServiceProviderDto> GetServiceProviderByIdAsync(Guid id)
        {
            _logger.LogInformation("MaterialServiceProvider:GetServiceProviderByIdAsync:Method Start");
            MaterialServiceProviderDto materialServiceProviderDto = new MaterialServiceProviderDto();
            try
            {
                var serviceprovider = await _genericRepo.Query()
                    .Include(x=> x.Contractor)
                    .Include(x => x.Manufacturer).FirstOrDefaultAsync(x=>x.Id == id).ConfigureAwait(false);    
                materialServiceProviderDto =_mapper.Map<MaterialServiceProviderDto>(serviceprovider);
              // var Serviceprovider = _mapper.Map<MaterialServiceProvider>(materialServiceProviderDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialServiceProvider:GetServiceProviderByIdAsync:Method End");
            return materialServiceProviderDto;
        }

        public async Task<MaterialServiceProviderDto> UpdateServiceProviderAsync(MaterialServiceProviderDto serviceProviderDto)
        {
            _logger.LogInformation("MaterialServiceProvider:UpdateServiceProviderAsync:Method Start");
            try
            {
                var existingServiceProvider = await _genericRepo.GetById(serviceProviderDto.Id);
                if (existingServiceProvider != null)
                {
                    if (!string.IsNullOrEmpty(serviceProviderDto.ServiceProviderDocument))
                    {
                        existingServiceProvider.ServiceProviderDocument = serviceProviderDto.ServiceProviderDocument;
                    }
                    existingServiceProvider.UpdatedDate = DateTime.Now;
                    existingServiceProvider.ServiceProviderName = serviceProviderDto.ServiceProviderName;
                    existingServiceProvider.Address = serviceProviderDto.Address;
                    existingServiceProvider.ContactNo = serviceProviderDto.ContactNo;
                    existingServiceProvider.ContactEmail = serviceProviderDto.ContactEmail;
                    existingServiceProvider.Remarks = serviceProviderDto.Remarks;
                }

                await _genericRepo.Update(existingServiceProvider);
            }
            
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialServiceProvider:UpdateServiceProviderAsync:Method End");
            return serviceProviderDto;
        }
    }
}
