using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationOperationController : ControllerBase
    {
        private readonly ILocationOperationService _locationOperationService;
        public LocationOperationController(ILocationOperationService locationOperationService)
        {
            _locationOperationService = locationOperationService;
            
        }
        [HttpGet("GetAllLocationOperation")]
        public async Task<IActionResult> GetAllLocationOperation()
        {
            var listLocationOperation = await _locationOperationService.GetAllLocationOperation();
            return Ok(listLocationOperation);
        }
        [HttpGet("GetLocationOperation")]
        public async Task<IActionResult> GetLocationOperation(Guid id)
        {
            var listLocationOperation = await _locationOperationService.GetLocationOperation(id);
            return Ok(listLocationOperation);
        }
        [HttpGet("GetLocationsByWorkPackage")]
        public async Task<IActionResult> GetLocationsByWorkPackage(Guid workPackageId)
        {
            var listLocationOperation = await _locationOperationService.GetLocationsByWorkPackageId(workPackageId);
            return Ok(listLocationOperation);
        }
        [HttpGet("GetLocationsBySubDivisionId")]
        public async Task<IActionResult> GetLocationsBySubDivisionId(Guid subDivisionId)
        {
            var listLocationOperation = await _locationOperationService.GetLocationsBySubDivisionId(subDivisionId);
            return Ok(listLocationOperation);
        }
        [HttpPost]
        public async Task<IActionResult> AddLocationOperation(LocationOperationDto locationOperationDto)
        {
            return Ok(await _locationOperationService.AddLocationOperation(locationOperationDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLocationOperation(LocationOperationDto locationOperationDto)
        {
            return Ok(await _locationOperationService.UpdateLocationOperation(locationOperationDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLocationOperation(Guid id)
        {
            await _locationOperationService.DeleteLocationOperation(id);
            return Ok();
        }

    }
}
