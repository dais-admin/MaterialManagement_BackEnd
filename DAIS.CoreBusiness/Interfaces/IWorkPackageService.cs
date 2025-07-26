
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IWorkPackageService
    {
        Task<WorkPackageDto> GetWorkPackage(Guid id);
        Task<WorkPackageDto> AddWorkPackage(WorkPackageDto workPackageDto);
        Task<WorkPackageDto> UpdateWorkPackage(WorkPackageDto workPackageDto);
        Task DeleteWorkPakage(Guid id);
        Task<List<WorkPackageDto>> GetAllWorkPackages();
        Task<List<WorkPackageDto>> GetWorkPackagesByProjectId(Guid projectId);
        Task<List<WorkPackageDto>> GetWorkPackagesByProjectIdAndSystem(Guid projectId,string system);
        WorkPackageDto GetWorkPackageIdByName(string name);
    }
}
