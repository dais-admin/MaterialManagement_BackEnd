﻿﻿﻿﻿using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MaterialTransferApprovalRequestDto = DAIS.CoreBusiness.Dtos.MaterialTransferApprovalRequestDto;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionMaterialTransferController : ControllerBase
    {
        private readonly IDivisionMaterialTransferService _divisionMaterialTransferService;
        public DivisionMaterialTransferController(IDivisionMaterialTransferService divisionMaterialTransferService)
        {
            _divisionMaterialTransferService = divisionMaterialTransferService;
        }
        [HttpPost]
        public async Task<ActionResult<DivisionMaterialTransferDto>> Create([FromBody] DivisionMaterialTransferDto divisionMaterialTransferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _divisionMaterialTransferService.AddDivisionMaterialTransfer(divisionMaterialTransferDto);
            return Ok(result);
        }
        [HttpGet("GetByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<DivisionMaterialTransferDto>> GetByVoucherNo(string voucherNo)
        {
            var result = await _divisionMaterialTransferService.GetDivisionMaterialTransferByVoucherNo(voucherNo);
            if (result == null)
            {
                return NotFound($"Material Issue Receive with voucher number '{voucherNo}' not found.");
            }
            return Ok(result);
        }
        [HttpGet("GetMaterialDivisionwiseIssueRecive")]
        public async Task<ActionResult<IEnumerable<DivisionMaterialIssueReceiveItemDto>>> GetMaterialDivisionwiseIssueRecive([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] Guid workPackageId)
        {
            if (fromDate > toDate)
            {
                return BadRequest("FromDate must be less than or equal to ToDate");
            }

            var result = await _divisionMaterialTransferService.GetDivisionMaterialIssueReceiveByDateRange(fromDate, toDate, workPackageId);
            return Ok(result);
        }

        [HttpPost("AddApproval")]
        public async Task<ActionResult> AddDivisionMaterialTransferApproval([FromBody] MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            

            var result = await _divisionMaterialTransferService.AddDivisionMaterialTransferApproval(materialTransferApprovalRequestDto);
            
            if (result)
            {
                return Ok(new { IsSucess = true, Message = "Approval record added successfully" });
            }
            else
            {
                return BadRequest(new { IsSucess = false, Message = "Failed to add approval record" });
            }
        }

        [HttpGet("GetByIssuingDivision/{divisionId}")]
        public async Task<ActionResult<IEnumerable<DivisionMaterialTransferApprovalDto>>> GetByIssuingDivision(Guid divisionId)
        {
            if (divisionId == Guid.Empty)
            {
                return BadRequest("Division ID is required");
            }

            var result = await _divisionMaterialTransferService.GetDivisionMaterialTransfersByIssuingDivision(divisionId);
            return Ok(result);
        }
        [HttpPut("updateStatus/{voucherNo}")]
        public async Task<IActionResult> UpdateItem(string voucherNo, [FromBody] ApprovalStatus status)
        {
            // Your logic to update the item with the given id and model data
            var result = await _divisionMaterialTransferService.UpdateDivisionMaterialTransferStatus(voucherNo, status);
            return Ok(new { IsSucess=true, Message = "Item updated successfully" });
        }
    }
}
