using DAIS.CoreBusiness.Dtos;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetSupplier(Guid id);
        Task<SupplierDto> AddSupplier(SupplierDto supplierDto);
        Task<SupplierDto> UpdateSupplier(SupplierDto supplierDto);
        Task DeleteSupplier(Guid id);
        Task<List<SupplierDto>> GetAllSupplier();
        SupplierDto GetSupplierIdByName(string name);
    }
}
