﻿using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialTransferController : ControllerBase
    {
        private readonly IMaterialTransferService _materialTransferService;
        public MaterialTransferController(IMaterialTransferService materialTransferService)
        {
            _materialTransferService = materialTransferService;
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            var response= await _materialTransferService.AddMaterialTransferApproval(materialTransferApprovalRequestDto); 
            return Ok(response);
        }
        [HttpGet("GetMaterialTransferVoucherByStatus")]
        public async Task<IActionResult> GetMaterialTransferVoucherByStatus(ApprovalStatus approvalStatus)
        {
            var response = await _materialTransferService.GetAllMaterialIssueRecieveVoucherByStatus(approvalStatus);
            return Ok(response);
        }
        [HttpGet("GetByIssuingLocation/{locationId}")]
        public async Task<ActionResult<IEnumerable<LocationMaterialTransferDto>>> GetByIssuingLocation(Guid locationId)
        {
            if (locationId == Guid.Empty)
            {
                return BadRequest("Location ID is required");
            }

            var result = await _materialTransferService.GetLocationMaterialTransfersByIssuingLocation(locationId);
            return Ok(result);
        }
        [HttpPut("updateStatus/{voucherNo}")]
        public async Task<IActionResult> UpdateItem(string voucherNo, [FromBody] ApprovalStatus status)
        {
            // Your logic to update the item with the given id and model data
            var result = await _materialTransferService.UpdateLocationMaterialTransferStatus(voucherNo, status);
            return Ok(new {IsSucess=true, Message = "Item updated successfully" });
        }

        [HttpGet("GetUserMaterialTransfers/{userId}")]
        public async Task<ActionResult<UserMaterialTransferDto>> GetUserMaterialTransfers(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            var result = await _materialTransferService.GetUserMaterialTransfers(userId);
            return Ok(result);
        }
    }
}
