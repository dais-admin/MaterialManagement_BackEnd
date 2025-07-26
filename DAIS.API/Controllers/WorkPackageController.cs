using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkPackageController : ControllerBase
    {
        private readonly IWorkPackageService _workPackageService;
        public WorkPackageController(IWorkPackageService workPackageService)
        {
           _workPackageService = workPackageService;

        }
        [HttpGet("GetAllWorkPackages")]
        public async Task<IActionResult> GetAllWorkPackages()
        {
            var listWorkPackags = await _workPackageService.GetAllWorkPackages();
            return Ok(listWorkPackags);

        }
        [HttpGet("GetWorkPackage")]
        public async Task<IActionResult> GetWorkPackage(Guid id)
        {
            var workPackage = await _workPackageService.GetWorkPackage(id);
            return Ok(workPackage);
        }
        [HttpGet("GetWorkPackagesByProjectId")]
        public async Task<IActionResult> GetWorkPackagesByProjectId(Guid projectId)
        {
            var workPackage = await _workPackageService.GetWorkPackagesByProjectId(projectId);
            return Ok(workPackage);
        }
        [HttpGet("GetWorkPackagesByProjectAndSystem")]
        public async Task<IActionResult> GetWorkPackagesByProjectAndSystem(Guid projectId,string system)
        {
            var workPackages = await _workPackageService.GetWorkPackagesByProjectIdAndSystem(projectId,system);
            return Ok(workPackages);
        }
        [HttpPost]
        public async Task<IActionResult> AddWorkPackage(WorkPackageDto workPackageDto)
        {
            return Ok(await _workPackageService.AddWorkPackage(workPackageDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkPackage(WorkPackageDto workPackageDto)
        {
            return Ok(await _workPackageService.UpdateWorkPackage(workPackageDto));
        }

        [HttpDelete]
        public async Task<IActionResult> UpdateWorkPackage(Guid id)
        {
            await _workPackageService.DeleteWorkPakage(id);
            return Ok();
        }
    }
}
