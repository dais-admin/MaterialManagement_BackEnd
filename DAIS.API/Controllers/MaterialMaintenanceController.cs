using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialMaintenanceController : ControllerBase
    {
        private readonly IMaterialMaintenanceService _materialMaintenanceService;
        public MaterialMaintenanceController(IMaterialMaintenanceService materialMaintenanceService)
        {
            _materialMaintenanceService = materialMaintenanceService;
            
        }
        [HttpGet("GetAllMaterialMaintenance")]
        public async Task<IActionResult> GetAllMaterialMaintenacesAsync()
        {
            var listMaterialMaintenance = await _materialMaintenanceService.GetAllMaterialMaintenacesAsync();
            return Ok(listMaterialMaintenance);
        }
        [HttpGet("GetMaterialMaintenanceById")]
        public async Task<IActionResult> GetMaterialMaintenaceByIdAsync(Guid id)
        {
            var listMaterialMaintenance = await _materialMaintenanceService.GetMaterialMaintenaceByIdAsync(id);
            return Ok(listMaterialMaintenance);
        }
        [HttpGet("GetMaintenanceByMaterialIdAsync")]
        public async Task<IActionResult> GetMaintenanceByMaterialIdAsync(Guid materialId)
        {
            var maintenanceDto = await _materialMaintenanceService.GetMaintenanceByMaterialIdAsync(materialId);
            return Ok(maintenanceDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterialMaintenanceAsync(MaterialMaintenaceDto materialMaintenaceDto)
        {
            return Ok(await _materialMaintenanceService.AddMaterialMaintenanceAsync(materialMaintenaceDto));

        }
        [HttpPut]
        public async Task<IActionResult> UpdateMaterialMaintenaceAsync(MaterialMaintenaceDto materialMaintenaceDto)
        {
            return Ok(await _materialMaintenanceService.UpdateMaterialMaintenaceAsync(materialMaintenaceDto));
               
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialMaintenaceAsync(Guid id)
        {
            await _materialMaintenanceService.DeleteMaterialMaintenaceAsync(id);
            return Ok();
        }
    }
}
