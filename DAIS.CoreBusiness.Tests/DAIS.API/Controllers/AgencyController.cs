﻿using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgencyController : ControllerBase
    {
        private  readonly IAgencyService _agencyService;
        public AgencyController(IAgencyService agencyService) 
        { 
            _agencyService = agencyService;
        
        }
        [HttpGet("GetAllAgency")]
        public async Task<IActionResult> GetAllAgency()
      {
            var listAgency = await _agencyService.GetAllAgency();
            return Ok(listAgency);

        }
        [HttpGet("GetAgency")]
        public async Task<IActionResult> GetAgency(Guid id)
        {
            var agency = await _agencyService.GetAgency(id);
            if (agency == null)
                return NotFound(new { message = "Agency not found" });
            return Ok(agency);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgency(AgencyDto agency)
        {
            var result = await _agencyService.AddAgency(agency);
            if (result == null)
                return BadRequest(new { message = "Failed to create agency" });
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAgency(AgencyDto agencyDto)
        {
            var result = await _agencyService.UpdateAgency(agencyDto);
            if (result == null)
                return NotFound(new { message = "Agency not found" });
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAgency(Guid id)
        {
            try
            {
                await _agencyService.DeleteAgency(id);
                return Ok(new { message = "Agency deleted successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500,"You can not delete this record");
            }
            
        }
    }
}
