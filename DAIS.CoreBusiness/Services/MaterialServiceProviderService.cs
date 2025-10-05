using AutoMapper;
using DAIS.CoreBusiness.Dtos;
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

        public MaterialServiceProviderService(IGenericRepository<MaterialServiceProvider> genericRepo, IMapper mapper, ILogger<MaterialServiceProviderService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
            
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
                var serviceprovider = await _genericRepo.GetById(id);
                await _genericRepo.Remove(serviceprovider);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialServiceProvider:DeleteServiceProviderAsync:Method End");

        }

        public async Task<List<MaterialServiceProviderDto>> GetAllServiceProviderAsync()
        {
            _logger.LogInformation("MaterialServiceProvider:GetAllServiceProviderAsync:Method Start");
            List<MaterialServiceProviderDto> materialServiceProviderDtoList= new List<MaterialServiceProviderDto>();
            try
            {
                
                var serviceProviderList = await _genericRepo.Query()
                    .Include(x=>x.Contractor)
                     .Include(x => x.Manufacturer).ToListAsync().ConfigureAwait(false);
                materialServiceProviderDtoList.AddRange(_mapper.Map<List<MaterialServiceProviderDto>>(serviceProviderList));    
               


            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialServiceProvider:GetAllServiceProviderAsync:Method End"); ;
            return materialServiceProviderDtoList;

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
