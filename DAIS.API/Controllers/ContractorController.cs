using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractorController : ControllerBase
    {
        private readonly IContractorService _contractorService;
        private readonly IFileManagerService _fileManagerService;
        private readonly string folderName = "MaterialDocument" + "\\" + "ContractorDocuments" + "\\";

        public ContractorController(IContractorService contractorService,
            IFileManagerService fileManagerService)
        {
            _contractorService = contractorService;
            _fileManagerService = fileManagerService;
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
        public async Task<IActionResult> AddContractor(List<IFormFile> contractorDocuments, [FromForm] string contractorData)
        {
            if (contractorData == null || string.IsNullOrEmpty(contractorData))
            {
                return BadRequest();
            }
            var contractorDto = JsonConvert.DeserializeObject<ContractorDto>(contractorData);
            if (contractorDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var supplierDocumentFile in contractorDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(supplierDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                contractorDto.ContractorDocument = uploadedFiles.ToString();
            }
            
            var response = await _contractorService.AddContractor(contractorDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContractor(List<IFormFile> contractorDocuments, [FromForm] string contractorData)
        {
            if (contractorData == null || string.IsNullOrEmpty(contractorData))
            {
                return BadRequest();
            }
            var contractorDto = JsonConvert.DeserializeObject<ContractorDto>(contractorData);
            if (contractorDocuments.Count>0)
            {
                StringBuilder uploadedFiles = new StringBuilder();
                foreach (var supplierDocumentFile in contractorDocuments)
                {
                    var (isSucess, savedFile) = await _fileManagerService.UploadAndEncryptFile(supplierDocumentFile, folderName);
                    uploadedFiles.Append(savedFile);
                    uploadedFiles.Append(";");
                }
                contractorDto.ContractorDocument = uploadedFiles.ToString();
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
        [HttpGet("download-encrypted/{encodedPath}")]
        public async Task<IActionResult> DownloadEncrypted(string encodedPath)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedPath);
            var decodedPath = Encoding.UTF8.GetString(base64EncodedBytes);
            var (stream, isSicess) = await _fileManagerService.GetEncryptedFile(decodedPath);

            if (!isSicess)
                return NotFound("File not found.");

            return File(stream, "application/octet-stream", decodedPath);
        }
    }
}

