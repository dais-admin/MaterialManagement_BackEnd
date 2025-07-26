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
    public class MaterialMeasurementController : ControllerBase
    {
        private readonly IMaterialMeasurementService _materialMeasurementService;
        public MaterialMeasurementController(IMaterialMeasurementService materialMeasurementService)
        {
            _materialMeasurementService = materialMeasurementService;
        }
       [HttpGet("GetAllMeasurement")]
       public async Task<IActionResult> GetAllMaterialMeasurement()
       {
           var listMeasurement = await _materialMeasurementService.GetAllMaterialMeasurement();
            return Ok(listMeasurement);

       }
      [HttpGet("GetMaterialMeasurement")]
      public async Task<IActionResult> GetMaterialMeasurement(Guid id)
      {
           var listMeasurement = await _materialMeasurementService.GetMaterialMeasurement(id);
           return Ok(listMeasurement);
      }
      [HttpPost]
      public async Task<IActionResult> AddMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto)
      {
        return Ok(await _materialMeasurementService.AddMaterialMeasurement(materialMeasuremetDto));
      }

      [HttpPut]
      public async Task<IActionResult> UpdateMaterialMeasurement(MaterialMeasuremetDto materialMeasuremetDto)
      {
           return Ok(await _materialMeasurementService.UpdateMaterialMeasurement(materialMeasuremetDto));
      }

      [HttpDelete]
      public async Task<IActionResult> DeleteMeasurement(Guid id)
      {
         await _materialMeasurementService.DeleteMeasurement(id);
         return Ok();
      }

    }
    
}
