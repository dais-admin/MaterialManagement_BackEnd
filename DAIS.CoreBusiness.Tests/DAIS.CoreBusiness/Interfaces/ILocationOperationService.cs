using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface ILocationOperationService
    {

        Task<LocationOperationDto> GetLocationOperation(Guid id);
        Task<LocationOperationDto>AddLocationOperation(LocationOperationDto locationOperationdto);
        Task DeleteLocationOperation(Guid id);
        Task<LocationOperationDto> UpdateLocationOperation(LocationOperationDto locationOperationdto);
        Task<List<LocationOperationDto>> GetAllLocationOperation();
        Task<List<LocationOperationDto>> GetLocationsByWorkPackageId(Guid workPackageId);
        LocationOperationDto GetLocationIdByName(string name, string system);
        Task<List<LocationOperationDto>> GetLocationsBySubDivisionId(Guid subDivisionId);
    }
}
