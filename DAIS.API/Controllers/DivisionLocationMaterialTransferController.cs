using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionLocationMaterialTransferController : ControllerBase
    {
        private readonly IDivisionLocationMaterialTransferService _divisionLocationMaterialTransferService;
        
        public DivisionLocationMaterialTransferController(IDivisionLocationMaterialTransferService divisionLocationMaterialTransferService)
        {
            _divisionLocationMaterialTransferService = divisionLocationMaterialTransferService;
        }
        
        [HttpPost]
        public async Task<ActionResult<DivisionLocationMaterialTransferDto>> Create([FromBody] DivisionLocationMaterialTransferDto divisionLocationMaterialTransferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _divisionLocationMaterialTransferService.AddDivisionLocationMaterialTransfer(divisionLocationMaterialTransferDto);
            return Ok(result);
        }
        
        [HttpGet("GetByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<DivisionLocationMaterialTransferDto>> GetByVoucherNo(string voucherNo)
        {
            var result = await _divisionLocationMaterialTransferService.GetDivisionLocationMaterialTransferByVoucherNo(voucherNo);
            if (result == null)
            {
                return NotFound($"Division Location Material Transfer with voucher number '{voucherNo}' not found.");
            }
            return Ok(result);
        }
        
        [HttpGet("GetMaterialDivisionLocationwiseIssueRecive")]
        public async Task<ActionResult<IEnumerable<DivisionLocationMaterialIssueReceiveItemDto>>> GetMaterialDivisionLocationwiseIssueRecive(
            [FromQuery] DateTime fromDate, 
            [FromQuery] DateTime toDate, 
            [FromQuery] Guid workPackageId)
        {
            if (fromDate > toDate)
            {
                return BadRequest("FromDate must be less than or equal to ToDate");
            }

            var result = await _divisionLocationMaterialTransferService.GetDivisionLocationMaterialIssueReceiveByDateRange(fromDate, toDate, workPackageId);
            return Ok(result);
        }
        [HttpPost("AddApproval")]
        public async Task<ActionResult> AddDivisionLocationMaterialTransferApproval([FromBody] MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            
            var result = await _divisionLocationMaterialTransferService.AddDivisionLocationMaterialTransferApproval(materialTransferApprovalRequestDto);

            if (result)
            {
                return Ok(new { Success = true, Message = "Approval record added successfully" });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Failed to add approval record" });
            }
        }

        [HttpGet("GetByIssuingDivision/{divisionId}")]
        public async Task<ActionResult<IEnumerable<SubDivisionMaterialTransferApprovalDto>>> GetByIssuingDivision(Guid divisionId)
        {
            if (divisionId == Guid.Empty)
            {
                return BadRequest("DivisionId ID is required");
            }

            var result = await _divisionLocationMaterialTransferService.GetDivisionLocationMaterialTransfersByIssuingDivision(divisionId);
            return Ok(result);
        }
        [HttpPut("updateStatus/{voucherNo}")]
        public async Task<IActionResult> UpdateItem(string voucherNo, [FromBody] ApprovalStatus status)
        {
            // Your logic to update the item with the given id and model data
            var result = await _divisionLocationMaterialTransferService.UpdateDivisionLocationMaterialTransferStatus(voucherNo, status);
            return Ok(new { IsSucess = true, Message = "Item updated successfully" });
        }
    }
}
