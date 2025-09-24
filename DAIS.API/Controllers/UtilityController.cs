using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IApplicationDataBackupService _applicationDataBackupService;
        public UtilityController(IApplicationDataBackupService applicationDataBackupService)
        {
            _applicationDataBackupService = applicationDataBackupService;
        }
        [HttpGet("GetBackupDetails")]
        public async Task<IActionResult> Get()
        {
            var list= await _applicationDataBackupService.GetLastestBackupDetails();
            return Ok(list);
        }
    }
}
