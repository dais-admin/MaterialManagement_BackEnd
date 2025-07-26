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
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionService _divisionService;
        public DivisionController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
            
        }
        [HttpGet("GetAllDivision")]
        public async Task<IActionResult> GetAllDivision()
        {
            var listDivision = await _divisionService.GetAllDivision();
            return Ok(listDivision);
            
        }
        [HttpGet("GetDivision")]
        public async Task<IActionResult> GetDivision(Guid id)
        {
            var listDivision = await _divisionService.GetDivision(id);
            return Ok(listDivision);
        }
        [HttpGet("GetDivisionsByLocation")]
        public async Task<IActionResult> GetDivisionsByLocation(Guid locationId)
        {
            var listDivision = await _divisionService.GetDivisionsByLocation(locationId);
            return Ok(listDivision);
        }
        [HttpPost]
        public async Task<IActionResult> AddDivision(DivisionDto divisionDto)
        {
            return Ok(await _divisionService.AddDivision(divisionDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDivision(DivisionDto divisionDto)
        {
            return Ok(await _divisionService.UpdateDivision(divisionDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDivision(Guid id)
        {
            await _divisionService.DeleteDivision(id);
            return Ok();
        }
    }
}
