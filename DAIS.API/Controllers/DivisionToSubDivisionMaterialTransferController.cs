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
    public class DivisionToSubDivisionMaterialTransferController : ControllerBase
    {
        private readonly IDivisionToSubDivisionMaterialTransferService _divisionToSubDivisionMaterialTransferService;
        private readonly ILogger<DivisionToSubDivisionMaterialTransferController> _logger;

        public DivisionToSubDivisionMaterialTransferController(
            IDivisionToSubDivisionMaterialTransferService divisionToSubDivisionMaterialTransferService,
            ILogger<DivisionToSubDivisionMaterialTransferController> logger)
        {
            _divisionToSubDivisionMaterialTransferService = divisionToSubDivisionMaterialTransferService;
            _logger = logger;
        }

        [HttpGet("GetTransferByVoucherNo/{voucherNo}")]
        public async Task<ActionResult<DivisionToSubDivisionMaterialTransferDto>> GetDivisionToSubDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.GetDivisionToSubDivisionMaterialTransferByVoucherNo(voucherNo);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionToSubDivisionMaterialTransferByVoucherNo");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddTransfer")]
        public async Task<ActionResult<MaterialIssueReceiveResponseDto>> AddDivisionToSubDivisionMaterialTransfer(DivisionToSubDivisionMaterialTransferDto divisionToSubDivisionMaterialTransferDto)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.AddDivisionToSubDivisionMaterialTransfer(divisionToSubDivisionMaterialTransferDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddDivisionToSubDivisionMaterialTransfer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddTransferApproval")]
        public async Task<ActionResult<bool>> AddDivisionToSubDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.AddDivisionToSubDivisionMaterialTransferApproval(materialTransferApprovalRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddDivisionToSubDivisionMaterialTransferApproval");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("UpdateDivisionToSubDivisionMaterialTransferStatus")]
        public async Task<ActionResult<bool>> UpdateDivisionToSubDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.UpdateDivisionToSubDivisionMaterialTransferStatus(voucherNo, approvalStatus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateDivisionToSubDivisionMaterialTransferStatus");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetDivisionToSubDivisionMaterialTransfersByIssuingDivision/{divisionId}")]
        public async Task<ActionResult<List<DivisionToSubDivisionMaterialTransferApprovalDto>>> GetDivisionToSubDivisionMaterialTransfersByIssuingDivision(Guid divisionId)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.GetDivisionToSubDivisionMaterialTransfersByIssuingDivision(divisionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionToSubDivisionMaterialTransfersByIssuingDivision");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetMaterialTransferByDateRange")]
        public async Task<ActionResult<List<DivisionToSubDivisionMaterialIssueReceiveItemDto>>> GetDivisionToSubDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            try
            {
                var result = await _divisionToSubDivisionMaterialTransferService.GetDivisionToSubDivisionMaterialIssueReceiveByDateRange(fromDate, toDate, workPackageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionToSubDivisionMaterialIssueReceiveByDateRange");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
