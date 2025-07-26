using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialServiceProviderController : ControllerBase
    {
        private readonly IMaterialServiceProviderService _materialServiceProvider;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialServiceProviderController(IMaterialServiceProviderService materialServiceProvider,
             IOptions<MaterialConfigSettings> materialConfig)
        {
            _materialServiceProvider = materialServiceProvider;
            _materialConfig = materialConfig.Value;

        }
        [HttpGet("GetAllMaterialServiceProvides")]
        public async Task<IActionResult> GetAllMaterialServiceProvides()
        {
            var listMaterialService = await _materialServiceProvider.GetAllServiceProviderAsync();
            return Ok(listMaterialService);
        }
        [HttpGet("GetMaterialServiceProviderById")]
        public async Task<IActionResult> GetMaterialServiceProviderById(Guid id)
        {
            var listMaterialService = await _materialServiceProvider.GetServiceProviderByIdAsync(id);
            return Ok(listMaterialService);
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceProviderAsync([FromForm] IFormFile? serviceProviderDocument, [FromForm] string serviceProviderData)
        {
            if (serviceProviderData == null || string.IsNullOrEmpty(serviceProviderData))
            {
                return BadRequest();
            }
            var serviceProviderDto = JsonConvert.DeserializeObject<MaterialServiceProviderDto>(serviceProviderData);

            if (serviceProviderDocument != null)
            {
                serviceProviderDto.ServiceProviderDocument = await SaveServiceProviderDocument(serviceProviderDocument);
            }
            var response = await _materialServiceProvider.AddServiceProviderAsync(serviceProviderDto);
            return Ok(response);
        }
        

        [HttpPut]

        public async Task<IActionResult> UpdateServiceProviderAsync([FromForm] IFormFile? serviceProviderDocument, [FromForm] string serviceProviderData)
        {
            if (serviceProviderData == null || string.IsNullOrEmpty(serviceProviderData))
            {
                return BadRequest();
            }
            var serviceProviderDto = JsonConvert.DeserializeObject<MaterialServiceProviderDto>(serviceProviderData);

            if (serviceProviderDocument != null)
            {
                serviceProviderDto.ServiceProviderDocument = await SaveServiceProviderDocument(serviceProviderDocument);
            }
            var response = await _materialServiceProvider.UpdateServiceProviderAsync(serviceProviderDto);
            return Ok(response);


        }
        [HttpDelete]
        public async Task<IActionResult> DeleteServiceProviderAsync(Guid id)
        {
            await _materialServiceProvider.DeleteServiceProviderAsync(id);
            return Ok();


        }
        private async Task<string> SaveServiceProviderDocument([FromForm] IFormFile? documentFile)
        {
            var folderName = "MaterialDocument" + "\\"+ "ServiceProviderDocument"+"\\";
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
