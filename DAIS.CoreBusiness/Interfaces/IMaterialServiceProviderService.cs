using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialServiceProviderService
    {
        Task<MaterialServiceProviderDto> GetServiceProviderByIdAsync(Guid id);
        Task<MaterialServiceProviderDto> AddServiceProviderAsync(MaterialServiceProviderDto serviceProviderDto);
        Task<MaterialServiceProviderDto> UpdateServiceProviderAsync(MaterialServiceProviderDto serviceProviderDto);
        Task DeleteServiceProviderAsync(Guid id);
        Task<List<MaterialServiceProviderDto>> GetAllServiceProviderAsync();

    }
} 
