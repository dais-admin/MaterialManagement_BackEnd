using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
            
        }
        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var listProjects = await _projectService.GetAllProjects();
            return Ok(listProjects);
            
        }
        [HttpGet("GetProject")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectService.GetProject(id);
            return Ok(project);
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectDto projectDto)
        {
            return Ok(await _projectService.AddProject(projectDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(ProjectDto projectDto)
        {
            return Ok(await _projectService.UpdateProject(projectDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            await _projectService.DeleteProject(id);
            return Ok();
        }
    }
}
