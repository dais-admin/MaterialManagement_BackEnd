using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
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
    public class ContractorController : ControllerBase
    {
        private readonly IContractorService _contractorService;
        private readonly MaterialConfigSettings _materialConfig;
        public ContractorController(IContractorService contractorService, IOptions<MaterialConfigSettings> materialConfig)
        {
            _contractorService = contractorService;
            _materialConfig = materialConfig.Value;
        }
        [HttpGet("GetAllContractor")]
        public async Task<IActionResult> GetAllContractor()
        {
            var listContractor = await _contractorService.GetAllContractor();
            return Ok(listContractor);
        }
        [HttpGet("GetContractor")]
        public async Task<IActionResult> GetContractor(Guid id)
        {
            var listContractor = await _contractorService.GetContractor(id);
            return Ok(listContractor);
        }
        [HttpPost]
        public async Task<IActionResult> AddContractor([FromForm] IFormFile? contractorDocument, [FromForm] string contractorData)
        {
            if (contractorData == null || string.IsNullOrEmpty(contractorData))
            {
                return BadRequest();
            }
            var contractorDto = JsonConvert.DeserializeObject<ContractorDto>(contractorData);

            if (contractorDocument != null)
            {
                contractorDto.ContractorDocument = await SaveContractorDocument(contractorDocument);
            }
            var response = await _contractorService.AddContractor(contractorDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContractor([FromForm] IFormFile? contractorDocument, [FromForm] string contractorData)
        {
            if (contractorData == null || string.IsNullOrEmpty(contractorData))
            {
                return BadRequest();
            }
            var contractorDto = JsonConvert.DeserializeObject<ContractorDto>(contractorData);

            if (contractorDocument != null)
            {
                contractorDto.ContractorDocument = await SaveContractorDocument(contractorDocument);
            }
            var response = await _contractorService.UpdateContractor(contractorDto);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContractor(Guid id)
        {
            await _contractorService.DeleteContractor(id);
            return Ok();
        }
        private async Task<string> SaveContractorDocument([FromForm] IFormFile? documentFile)
        {
            var folderName = "MaterialDocument" + "\\" + "ContractorDocuments" + "\\";
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

