using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialApprovalController : ControllerBase
    {
        private readonly IMaterialApprovalService _materialApprovalService;
        private readonly IApprovalStatusHistoryService _approverStatusHistoryService;
        private readonly MaterialConfigSettings _materialConfig;
        public MaterialApprovalController(IMaterialApprovalService materialApprovalService,
            IOptions<MaterialConfigSettings> materialConfig,
            IApprovalStatusHistoryService approverStatusHistoryService)
        {
            _materialApprovalService = materialApprovalService;
            _materialConfig = materialConfig.Value;
            _approverStatusHistoryService = approverStatusHistoryService;
        }
        [HttpPost("AddApproval")]
        public async Task<ActionResult<MaterialApprovalDto>> AddApproval(ApprovalInformationDto approvalInformationDto)
        {
            var materialApprovalDto = await _materialApprovalService.AddMaterialApproval(approvalInformationDto);
            return Ok(materialApprovalDto);
        }
        

        [HttpPut("UpdateApproval")]
        public async Task<ActionResult<MaterialApprovalDto>> UpdateApproval(ApprovalInformationDto approvalInformationDto)
        {
            var materialApprovalDto = await _materialApprovalService.UpdateMaterialApprovalStatus(approvalInformationDto);
            return Ok(materialApprovalDto);
        }
        [HttpPut("UpdateBulkApproval")]
        public async Task<ActionResult> UpdateBulkApproval([FromForm] IFormFile? bulkUploadFile, [FromForm] string bulkApprovalInfo)
        {
            if (bulkApprovalInfo == null || string.IsNullOrEmpty(bulkApprovalInfo))
            {
                return BadRequest();
            }
            var bulkApprovalInfoDto = JsonConvert.DeserializeObject<BulkApprovalInformationDto>(bulkApprovalInfo);
            //var bulkUploadFile = Request.Form.Files;
            if (bulkUploadFile != null)
            {
                await SaveBulkUploadFile(bulkUploadFile, bulkApprovalInfoDto.BulkUploadFileName);
            }
            var materialApprovalDto = await _materialApprovalService.UpdateMaterialBulkApprovalStatus(bulkApprovalInfoDto);
            return Ok(materialApprovalDto);
        }
        [HttpGet("GetMaterialsByStatus")]
        public async Task<IActionResult> GetMaterialsForReview(ApprovalStatus approvalStatus, bool isActive,string userId)
        {
            var listToReview=await _materialApprovalService.GetMaterialsByStatusAsync(approvalStatus,isActive,userId);
            return Ok(listToReview);
        }
              
        [HttpGet("GetMaterialApprovalListByUserId")]
        public async Task<IActionResult> GetMaterialListByUserId(string userId,string userRole)
        {
            var materialList = await _materialApprovalService.GetMaterialListByUserIdAsync(userId,userRole);
            return Ok(materialList);
        }
        [HttpGet("GetMateriaApprovalByMaterialId")]
        public async Task<IActionResult> GetMateriaApprovalByMaterialId(string materialId)
        {
            var materialAprovaldto = await _materialApprovalService.GetMaterialApproveByMaterialId(Guid.Parse(materialId));
            return Ok(materialAprovaldto);
        }
        [HttpGet("GetMateriaApprovalByWorkPackageId")]
        public async Task<IActionResult> GetMateriaApprovalByWorkPackageId(Guid workPackageId)
        {
            var materialAprovaldto = await _materialApprovalService.GetAllMaterialApprovals(workPackageId);
            return Ok(materialAprovaldto);
        }

        [HttpGet("GetBulkApprovalMaterialListByUserId")]
        public async Task<IActionResult> GetBulkApprovalMaterialListByUserId(string userId, string userRole)
        {
            var materialList = await _materialApprovalService.GetBulkApprovalMaterialListByUserId(userId, userRole);
            
            return Ok(materialList);
        }

        //New Code 
        [HttpPost("AddBulkApproval")]
        public async Task<ActionResult<MaterialApprovalDto>> AddBulkApproval(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            var materialApprovalDto = await _approverStatusHistoryService.AddBulkApprovalStatusHistory(bulkApprovalInformationDto);
            return Ok(materialApprovalDto);
        }
        [HttpPost("AddApprovalStausHistory")]
        public async Task<IActionResult> AddApprovalStatusHistory(ApprovalStatusHistoryDto approvalStatusHistoryDto)
        {
            var result = await _approverStatusHistoryService.AddApprovalStatusHistory(approvalStatusHistoryDto);
            return Ok(result);
        }
        [HttpGet("GetMaterialsApprovalStatusByUser")]
        public async Task<IActionResult> GetMaterialsApprovalStatusByUser(string approvalStatus, string userEmail)
        {
            var statuses = approvalStatus?.Split(',');
            var listToReview = await _approverStatusHistoryService.GetMaterialsWithStatusHistoryByUser(statuses.ToList(), userEmail);
            return Ok(listToReview);
        }
        [HttpGet("GetBulkUploadMaterialListByUser")]
        public async Task<IActionResult> GetBulkUploadMaterialListByUser(string approvalStatus, string userEmail)
        {
            var statuses = approvalStatus?.Split(',');
            var bulkUplloadDetailList = await _approverStatusHistoryService.GetBulkApprovalMaterialsWithStatusHistoryByUser(statuses.ToList(), userEmail);
            return Ok(bulkUplloadDetailList);
        }
        [HttpGet("GetMaterialApprovalListByStatus")]
        public async Task<IActionResult> GetMaterialApprovalListByStatus(string approvalStatus, Guid workpackageId, Guid? locationId)
        {
            var materialApprovalList = await _approverStatusHistoryService.GetMaterialsStatusByProjectWorkpackage(approvalStatus, workpackageId, locationId);
            return Ok(materialApprovalList);
        }


        private async Task<string> SaveBulkUploadFile([FromForm] IFormFile? bulkUploadFile, string fileName)
        {
            string fullFilePath=string.Empty;
            try
            {
                var folderPath = _materialConfig.DocumentBasePath + "MaterialDocument//ExcelUploads//";
                //filePath = Path.Combine(folderName, fileName);
                fullFilePath = Path.Combine(folderPath, fileName);
                if (System.IO.File.Exists(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }
                await bulkUploadFile.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));
            }
            catch(Exception ex)
            {
                
            }
            

            return fullFilePath;
        }
    }
}
