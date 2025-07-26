using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;
        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet("GetRegionById")]
        public  async Task<ActionResult<RegionDto>> GetRegionById(Guid id)
        {
            return Ok(await _regionService.GetRegionById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddRegion(RegionDto regionDto)
        {
            return Ok(await _regionService.AddRegion(regionDto));
        }
       
        [HttpGet("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
             return Ok(await _regionService.GetAllRegions());
        }

        [HttpPut]
        public  async Task<IActionResult> UpdateRegion(RegionDto regionDto)
        {
            return Ok(await _regionService.UpdateRegion(regionDto));
        }
        [HttpDelete]
        public  async Task<IActionResult> DeleteRegion(Guid id)
        {
            await _regionService.DeleteRegion(id);
            return Ok();
        }

    }
}
