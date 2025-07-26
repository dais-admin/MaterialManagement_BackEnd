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
    public class SubDivisionController : ControllerBase
    {
        private readonly ISubDivisionService _subDivisionService;
        public SubDivisionController(ISubDivisionService subDivisionService)
        {
            _subDivisionService = subDivisionService;


        }
        [HttpGet("GetAllSubDivision")]
        public async Task<IActionResult> GetAllSubDivision()
        {
            var listSubDivision = await _subDivisionService.GetAllSubDivision();
            return Ok(listSubDivision);
        }
        [HttpGet("GetSubDivisionsByDivision")]
        public async Task<IActionResult> GetSubDivisionsByDivision(Guid divisionId)
        {
            var listSubDivision = await _subDivisionService.GetAllSubDivisionsByDivision(divisionId);
            return Ok(listSubDivision);
        }
        [HttpGet("GetSubDivision")]
        public async Task<IActionResult> GetSubDivision(Guid id)
        {
            var listSubDivision = await _subDivisionService.GetSubDivision(id);
            return Ok(listSubDivision);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubDivision(SubDivisionDto subDivisionDto)
        {
            return Ok(await _subDivisionService.AddSubDivision(subDivisionDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubDivision(SubDivisionDto subDivisionDto)
        {
            return Ok(await _subDivisionService.UpdateSubDivision(subDivisionDto));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubDivision(Guid id)
        {
            await _subDivisionService.DeleteSubDivision(id);
            return Ok();
        }
    }
}


