using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MaterialTypeController : ControllerBase
    {
        private readonly IMaterialTypeService _materialTypeService;
        public MaterialTypeController(IMaterialTypeService materialTypeService)
        {
            _materialTypeService = materialTypeService;
        }
        [HttpGet("GetAllMaterialTypes")]
        public async Task<IActionResult> GetAllMaterialTypes()
        {
            var listMaterialTYpes = await _materialTypeService.GetAllMaterialTypes();
            return Ok(listMaterialTYpes);
        }
        [HttpGet("GetAllMaterialType")]
        public async Task<IActionResult> GetAllMaterialType(Guid typeId)
        {
            var listMaterialTYpes = await _materialTypeService.GetMaterialTypeById(typeId);
            return Ok(listMaterialTYpes);
        }
        [HttpPost]
        public async Task<IActionResult> AddMateialType(MaterialTypeDto materialTypeDto)
        {
            return Ok(await _materialTypeService.AddMaterialType(materialTypeDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMateialType(MaterialTypeDto materialTypeDto)
        {
            return Ok(await _materialTypeService.UpdateMaterialType(materialTypeDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMateialType(Guid typeId)
        {
            await _materialTypeService.DeleteMaterialType(typeId);
            return Ok();
        }
    }
}
