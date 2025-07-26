using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialIssueReceiveController : ControllerBase
    {
        private readonly IMaterialIssueReceiveService _materialIssueReceiveService;

        public MaterialIssueReceiveController(IMaterialIssueReceiveService materialIssueReceiveService)
        {
            _materialIssueReceiveService = materialIssueReceiveService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<MaterialIssueReceiveDto>>> GetAll()
        {
            var result = await _materialIssueReceiveService.GetAllMaterialIssueReceive();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialIssueReceiveDto>> Get(Guid id)
        {
            var result = await _materialIssueReceiveService.GetMaterialIssueReceive(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<MaterialIssueReceiveDto>> GetByVoucherNo(string voucherNo)
        {
            var result = await _materialIssueReceiveService.GetMaterialIssueReceiveByVoucherNo(voucherNo);
            if (result == null)
            {
                return NotFound($"Material Issue Receive with voucher number '{voucherNo}' not found.");
            }
            return Ok(result);
        }

        [HttpGet("GetByDateRange")]
        public async Task<ActionResult<IEnumerable<MaterialIssueReceiveDto>>> GetByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return BadRequest("FromDate must be less than or equal to ToDate");
            }

            var result = await _materialIssueReceiveService.GetMaterialIssueReceiveByDateRange(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("GetMaterialLocationIssueRecive")]
        public async Task<ActionResult<IEnumerable<MaterialIssueReceiveDto>>> GetMaterialLocationIssueRecive([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] Guid workPackageId)
        {
            if (fromDate > toDate)
            {
                return BadRequest("FromDate must be less than or equal to ToDate");
            }

            var result = await _materialIssueReceiveService.GetMaterialLocationIssueReceiveByDateRange(fromDate, toDate, workPackageId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<MaterialIssueReceiveDto>> Create([FromBody] MaterialIssueReceiveDto materialIssueReceiveDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _materialIssueReceiveService.AddMaterialIssueReceive(materialIssueReceiveDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MaterialIssueReceiveDto>> Update(Guid id, [FromBody] MaterialIssueReceiveDto materialIssueReceiveDto)
        {
            if (id != materialIssueReceiveDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _materialIssueReceiveService.UpdateMaterialIssueReceive(materialIssueReceiveDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _materialIssueReceiveService.DeleteMaterialIssueReceive(id);
            return NoContent();
        }
    }
}
