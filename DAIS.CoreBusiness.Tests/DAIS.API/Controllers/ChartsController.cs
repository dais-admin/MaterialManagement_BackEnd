using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartService _chartService;
        public ChartsController(IChartService chartService)
        {
            _chartService = chartService;
        }
        [HttpGet("GetChartDataByWorkPackage")]
        public async Task<IActionResult> GetChartDataByWorkPackageId(Guid workPackageId)
        {
            
            return Ok(await _chartService.GetChartData(workPackageId));
        }
    }
}
