using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
namespace DAIS.CoreBusiness.Interfaces
{
    public interface IRegionService
    {
        Task<RegionDto> GetRegionById(Guid id);

        Task<RegionDto> AddRegion(RegionDto RegionDto);
        Task<RegionDto> UpdateRegion(RegionDto RegionDto);
        Task DeleteRegion(Guid id);
        Task<List<RegionDto>> GetAllRegions();
        RegionDto GetRegionIdByName(string name);
    }
}
