using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> GetProject(Guid id);
        Task<ProjectDto> AddProject(ProjectDto projectDto);
        Task<ProjectDto> UpdateProject(ProjectDto projectDto);
        Task DeleteProject(Guid id);
        Task<List<ProjectDto>> GetAllProjects();
        Task<ProjectDto> GetProjectIdByName(string name);

    }
}
