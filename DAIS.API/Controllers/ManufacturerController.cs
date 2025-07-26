using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly MaterialConfigSettings _materialConfig;
        public ManufacturerController(IManufacturerService manufacturerService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _manufacturerService = manufacturerService;
            _materialConfig = materialConfig.Value;

        }
        [HttpGet("GetAllManufacturer")]
        public async Task<IActionResult> GetAllManufacturer()
        {
            var listManufacturer = await _manufacturerService.GetAllManufacturer();
            return Ok(listManufacturer);
        }
        [HttpGet("GetManufacturer")]
        public async Task<IActionResult> GetManufacturer(Guid id)
        {
            var listManufacturer = await _manufacturerService.GetManufacturer(id);
            return Ok(listManufacturer);
        }
        [HttpPost]
        public async Task<IActionResult> AddManufacturer([FromForm] IFormFile? manufacturerDocument, [FromForm] string manufacturerData)
        {
            if (manufacturerData == null || string.IsNullOrEmpty(manufacturerData))
            {
                return BadRequest();
            }
            var ManufacturerDto = JsonConvert.DeserializeObject<ManufacturerDto>(manufacturerData);

            if (manufacturerDocument != null)
            {
                ManufacturerDto.ManufacturerDocument = await SavemanufacturerDocument(manufacturerDocument);
            }
            var response = await _manufacturerService.AddManufacturer(ManufacturerDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateManufacturer([FromForm] IFormFile? manufacturerDocument, [FromForm] string manufacturerData)
        {
            if (manufacturerData == null || string.IsNullOrEmpty(manufacturerData))
            {
                return BadRequest();
            }
            var ManufacturerDto = JsonConvert.DeserializeObject<ManufacturerDto>(manufacturerData);

            if (manufacturerDocument != null)
            {
                ManufacturerDto.ManufacturerDocument = await SavemanufacturerDocument(manufacturerDocument);
            }
            var response = await _manufacturerService.UpdateManufactuter(ManufacturerDto);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteManufacturer(Guid id)
        {
            await _manufacturerService.DeleteManufacturer(id);
            return Ok();
        }
        private async Task<string> SavemanufacturerDocument([FromForm] IFormFile? documentFile)
        {
            var folderName = "MaterialDocument" + "\\" + "ManufacturerDocument" + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = documentFile.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await documentFile.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }
    }
}
