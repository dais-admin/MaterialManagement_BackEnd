using DAIS.CoreBusiness.Dtos;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface IManufacturerService
    {
        Task<ManufacturerDto> GetManufacturer(Guid id);
        Task<ManufacturerDto> AddManufacturer(ManufacturerDto manufacturerDto);
        Task<ManufacturerDto> UpdateManufactuter(ManufacturerDto manufacturerDto);
        Task DeleteManufacturer(Guid id);
        Task<List<ManufacturerDto>> GetAllManufacturer();
        ManufacturerDto GetManufacturerIdByName(string name);
    }
}
