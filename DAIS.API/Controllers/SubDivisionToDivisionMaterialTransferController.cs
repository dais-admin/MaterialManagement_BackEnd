using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubDivisionToDivisionMaterialTransferController : ControllerBase
    {
        private readonly ISubDivisionToDivisionMaterialTransferService _subDivisionToDivisionMaterialTransferService;
        private readonly ILogger<SubDivisionToDivisionMaterialTransferController> _logger;

        public SubDivisionToDivisionMaterialTransferController(
            ISubDivisionToDivisionMaterialTransferService subDivisionToDivisionMaterialTransferService,
            ILogger<SubDivisionToDivisionMaterialTransferController> logger)
        {
            _subDivisionToDivisionMaterialTransferService = subDivisionToDivisionMaterialTransferService;
            _logger = logger;
        }

        [HttpGet("GetByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<SubDivisionToDivisionMaterialTransferDto>> GetSubDivisionToDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.GetSubDivisionToDivisionMaterialTransferByVoucherNo(voucherNo);
                if (result == null)
                {
                    return NotFound($"No material transfer found with voucher number: {voucherNo}");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving material transfer with voucher number: {voucherNo}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("AddTransfer")]
        public async Task<ActionResult<MaterialIssueReceiveResponseDto>> AddSubDivisionToDivisionMaterialTransfer([FromBody] SubDivisionToDivisionMaterialTransferDto subDivisionToDivisionMaterialTransferDto)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.AddSubDivisionToDivisionMaterialTransfer(subDivisionToDivisionMaterialTransferDto);
                if (!result.IsIssueReceiveSucess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding material transfer");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("Approval/Add")]
        public async Task<ActionResult<bool>> AddSubDivisionToDivisionMaterialTransferApproval([FromBody] MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.AddSubDivisionToDivisionMaterialTransferApproval(materialTransferApprovalRequestDto);
                if (!result)
                {
                    return BadRequest("Failed to add approval record");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding material transfer approval");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("Approval/Update/{voucherNo}/{approvalStatus}")]
        public async Task<ActionResult<bool>> UpdateSubDivisionToDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.UpdateSubDivisionToDivisionMaterialTransferStatus(voucherNo, approvalStatus);
                if (!result)
                {
                    return BadRequest("Failed to update approval status");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating material transfer status for voucher: {voucherNo}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("BySubDivision/{subDivisionId}")]
        public async Task<ActionResult<List<SubDivisionToDivisionMaterialTransferApprovalDto>>> GetSubDivisionToDivisionMaterialTransfersByIssuingSubDivision(Guid subDivisionId)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.GetSubDivisionToDivisionMaterialTransfersByIssuingSubDivision(subDivisionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving material transfers for subdivision ID: {subDivisionId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetMaterialTransferByDateRange")]
        public async Task<ActionResult<List<SubDivisionToDivisionMaterialIssueReceiveItemDto>>> GetSubDivisionToDivisionMaterialIssueReceiveByDateRange(
            [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] Guid workPackageId)
        {
            try
            {
                var result = await _subDivisionToDivisionMaterialTransferService.GetSubDivisionToDivisionMaterialIssueReceiveByDateRange(fromDate, toDate, workPackageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving material issue/receive report by date range");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
