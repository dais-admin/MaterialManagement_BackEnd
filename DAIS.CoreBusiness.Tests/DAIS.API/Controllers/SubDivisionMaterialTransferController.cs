using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubDivisionMaterialTransferController : ControllerBase
    {
        private readonly ISubDivisionMaterialTransferService _subDivisionMaterialTransferService;
        public SubDivisionMaterialTransferController(ISubDivisionMaterialTransferService subDivisionMaterialTransferService)
        {
            _subDivisionMaterialTransferService=subDivisionMaterialTransferService;
        }
        [HttpPost]
        public async Task<ActionResult<SubDivisionMaterialTransferDto>> Create([FromBody] SubDivisionMaterialTransferDto subDivisionMaterialTransferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subDivisionMaterialTransferService.AddSubDivisionMaterialTransfer(subDivisionMaterialTransferDto);
            return Ok(result);
        }
        [HttpGet("GetByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<SubDivisionMaterialTransferDto>> GetByVoucherNo(string voucherNo)
        {
            var result = await _subDivisionMaterialTransferService.GetSubDivisionMaterialTransferByVoucherNo(voucherNo);
            if (result == null)
            {
                return NotFound($"Material Issue Receive with voucher number '{voucherNo}' not found.");
            }
            return Ok(result);
        }
        [HttpGet("GetMaterialSubDivisionwiseIssueRecive")]
        public async Task<ActionResult<IEnumerable<SubDivisionMaterialIssueReceiveItemDto>>> GetMaterialSubDivisionwiseIssueRecive([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] Guid workPackageId)
        {
            if (fromDate > toDate)
            {
                return BadRequest("FromDate must be less than or equal to ToDate");
            }

            var result = await _subDivisionMaterialTransferService.GetSubDivisionMaterialIssueReceiveByDateRange(fromDate, toDate, workPackageId);
            return Ok(result);
        }
        [HttpPost("AddApproval")]
        public async Task<ActionResult> AddSubDivisionMaterialTransferApproval([FromBody] MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            
            var result = await _subDivisionMaterialTransferService.AddSubDivisionMaterialTransferApproval(materialTransferApprovalRequestDto);

            if (result)
            {
                return Ok(new { Success = true, Message = "Approval record added successfully" });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Failed to add approval record" });
            }
        }

        [HttpGet("GetByIssuingSubDivision/{subDivisionId}")]
        public async Task<ActionResult<IEnumerable<SubDivisionMaterialTransferApprovalDto>>> GetByIssuingSubDivision(Guid subDivisionId)
        {
            if (subDivisionId == Guid.Empty)
            {
                return BadRequest("SubDivisionId ID is required");
            }

            var result = await _subDivisionMaterialTransferService.GetSubDivisionMaterialTransfersByIssuingSubDivision(subDivisionId);
            return Ok(result);
        }
        [HttpPut("updateStatus/{voucherNo}")]
        public async Task<IActionResult> UpdateItem(string voucherNo, [FromBody] ApprovalStatus status)
        {
            // Your logic to update the item with the given id and model data
            var result = await _subDivisionMaterialTransferService.UpdateSubDivisionMaterialTransferStatus(voucherNo, status);
            return Ok(new { IsSucess = true, Message = "Item updated successfully" });
        }
    }
}
