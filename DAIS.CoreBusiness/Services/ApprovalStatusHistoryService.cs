
using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DAIS.CoreBusiness.Services
{
    public class ApprovalStatusHistoryService : IApprovalStatusHistoryService
    {
        private readonly IGenericRepository<ApprovalStatusHistory> _genericRepository;
        private readonly IGenericRepository<Material> _materialRepo;
        private readonly IGenericRepository<BulkUploadDetail> _bulkUploadDetailRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ApprovalStatusHistoryService> _logger;

        string actionRequiredByUserEmail = string.Empty;
        public ApprovalStatusHistoryService(IGenericRepository<ApprovalStatusHistory> genericRepository,
            IMapper mapper,ILogger<ApprovalStatusHistoryService> logger, IGenericRepository<Material> materialRepo
            , IGenericRepository<BulkUploadDetail> bulkUploadDetailRepo)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
            _materialRepo = materialRepo;
            _bulkUploadDetailRepo = bulkUploadDetailRepo;
        }
        public async Task<bool> AddApprovalStatusHistory(ApprovalStatusHistoryDto approvalStatusHistoryDto)
        {
            bool isSuccess = true;
            try
            {
                if(approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.ReviewerReturned.ToString())
                    || approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.ReviewerRejected.ToString())
                    || approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.ApproverReturened.ToString())
                    || approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.ApproverRejected.ToString()))
                {
                    var existngSubmittedmaterial = await _genericRepository.Query()
                        .Where(h => h.MaterialId == approvalStatusHistoryDto.MaterialId
                            && h.ApprovalStatus == ApprovalStatus.Submmitted)
                        .OrderByDescending(h => h.StatusChangeDate)
                        .FirstOrDefaultAsync();
                    
                    await AddApprovalStatusHistory(approvalStatusHistoryDto, existngSubmittedmaterial.StatusChangeBy);

                }
                if (approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.Approved.ToString()))
                {
                    await AddApprovalStatusHistory(approvalStatusHistoryDto, null);
                }
                if(approvalStatusHistoryDto.ReviewerApproverEmailIds!= null)
                {
                    foreach (string userEmail in approvalStatusHistoryDto.ReviewerApproverEmailIds)
                    {                       
                        await AddApprovalStatusHistory(approvalStatusHistoryDto, userEmail);
                    }
                }                             
            }
            catch (Exception ex)
            {
                isSuccess = false;
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return isSuccess;
        }

        public async Task<bool> AddBulkApprovalStatusHistory(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            bool isSuccess = true;
            
            try
            {
                

                if (bulkApprovalInformationDto.ApprovalStatus.Equals(ApprovalStatus.ReviewerReturned.ToString())
                    || bulkApprovalInformationDto.ApprovalStatus.Equals(ApprovalStatus.ReviewerRejected.ToString())
                    || bulkApprovalInformationDto.ApprovalStatus.Equals(ApprovalStatus.ApproverReturened.ToString())
                    || bulkApprovalInformationDto.ApprovalStatus.Equals(ApprovalStatus.ApproverRejected.ToString()))
                {

                    await AddBulkReturnRejectStatus(bulkApprovalInformationDto);

                }
                if (bulkApprovalInformationDto.ApprovalStatus.Equals(ApprovalStatus.Approved.ToString()))
                {
                    
                    await AddBulkApprovedStatus(bulkApprovalInformationDto);
                }
                if (bulkApprovalInformationDto.ReviewerApproverEmailIds != null)
                {
                    await AddBulkReviewedStatus(bulkApprovalInformationDto);
                }

                await UpdateBulkUploadDetailsStatus(bulkApprovalInformationDto,actionRequiredByUserEmail);
                var materialIds = await _materialRepo.Query()
                 .Where(m => m.BuilkUploadDetailId != null
                             && m.BuilkUploadDetailId == bulkApprovalInformationDto.BulkUploadDetailId)
                 .Select(m => m.Id)
                 .ToListAsync();

                foreach (var materialId in materialIds)
                {
                    await UpdateMaterialCurrentApprovalStatus(materialId, bulkApprovalInformationDto.ApprovalStatus);
                }

            }
            catch (Exception ex)
            {
                isSuccess = false;
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return isSuccess;
        }
        public async Task<IEnumerable<MaterialDto>> GetMaterialsWithStatusHistoryByUser(List<string>? approvalStatuses,string currentUserEmail)
        {
            IEnumerable<MaterialDto> marialListWithStatusHistory = new List<MaterialDto>();

            try
            {
                // Convert string list to enum list safely
                var statusEnums = approvalStatuses
                    .Where(s => Enum.TryParse(typeof(ApprovalStatus), s, true, out _))
                    .Select(s => (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), s, true))
                    .ToList();

                // ✅ 2. Load all status history entries (minimum filtered by status)
                var allHistories = await _genericRepository.Query()
                    .Where(x=>x.ActionRequiredByUserEmail == currentUserEmail)
                    .ToListAsync(); // We'll filter in-memory to safely group by material

                // ✅ 3. Group by materialId and pick only the latest status entry for each
                var materialIds = allHistories
                    .GroupBy(h => h.MaterialId)
                    .Select(g => g
                        .OrderByDescending(h => h.StatusChangeDate)
                        .FirstOrDefault()
                    )
                    .Where(h =>
                        h != null &&
                        h.ActionRequiredByUserEmail == currentUserEmail &&
                        statusEnums.Contains(h.ApprovalStatus)
                    )
                    .Select(h => h.MaterialId)
                    .Distinct()
                    .ToList();

                var materialList = await _materialRepo.Query()
                .Where(m => materialIds.Contains(m.Id) 
                && approvalStatuses.Contains(m.CurrentApprovalStatus))
                .Include(x => x.ApprovalStatusHistory)
                .ToListAsync();

                marialListWithStatusHistory = materialList.Select(material => new MaterialDto
                {
                    Id = material.Id,
                    MaterialName = material.MaterialName,
                    MaterialCode = material.MaterialCode,
                    TagNumber = material.TagNumber,
                    ModelNumber = material.ModelNumber,
                    MaterialQty = material.MaterialQty,
                    BuilkUploadDetailId = material.BuilkUploadDetailId,
                    CurrentApprovalStatus = material.CurrentApprovalStatus,
                    ApprovalStatusHistory = material.ApprovalStatusHistory.Select(history => new ApprovalStatusHistoryDto
                    {
                        StatusChangeBy = history.StatusChangeBy,
                        StatusChangeDate  = TimeZoneInfo.ConvertTimeFromUtc(
                            DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
                        ),
                       ApprovalStatus = ApprovalStatusDescriptions.GetDescription(history.ApprovalStatus),
                        Comments = history.Comments
                    }).ToList(),
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return marialListWithStatusHistory;
        }

        public async Task<IEnumerable<MaterialDto>> GetMaterialsStatusByProjectWorkpackage(string approvalStatus, Guid workpackageId,Guid? locationId)
        {
            IEnumerable<MaterialDto> marialListWithStatusHistory = new List<MaterialDto>();

            try
            {
                var materialListQuery = _materialRepo.Query()
                    .Where(x => x.WorkPackageId == workpackageId)                   
                    .Include(x => x.WorkPackage)                    
                    .AsQueryable();

                if (approvalStatus != null && approvalStatus !="0")
                {
                    materialListQuery = materialListQuery.Where(s => approvalStatus.Equals(s.CurrentApprovalStatus));
                }
                if (locationId != null && locationId != Guid.Empty)
                    materialListQuery = materialListQuery.Where(x => x.LocationId == locationId);
                           

                var materialList = await materialListQuery.ToListAsync();

                marialListWithStatusHistory = materialList.Select(material => new MaterialDto
                {
                    Id = material.Id,
                    MaterialName = material.MaterialName,
                    MaterialCode = material.MaterialCode,
                    TagNumber = material.TagNumber,
                    ModelNumber = material.ModelNumber,
                    MaterialQty = material.MaterialQty,
                    BuilkUploadDetailId = material.BuilkUploadDetailId,
                    CurrentApprovalStatus = material.CurrentApprovalStatus,
                    WorkPackageId = material.WorkPackageId,
                    UpdatedBy = material.UpdatedBy,
                   // UpdatedBy = material.CreatedBy,
                    UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
                    ),
                    WorkPackage = new WorkPackageDto
                    {
                        Id = material.WorkPackage.Id,
                        WorkPackageCode = material.WorkPackage.WorkPackageCode,
                        WorkPackageName = material.WorkPackage.WorkPackageName
                    }

                }); 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return marialListWithStatusHistory;
        }
        public async Task<IEnumerable<BulkUploadDetailsDto>> GetBulkApprovalMaterialsWithStatusHistoryByUser(List<string>? approvalStatuses, string currentUserEmail)
        {
            IEnumerable<BulkUploadDetailsDto> bulkUploadDetailsDtoList = new List<BulkUploadDetailsDto>();

            try
            {
                // Convert string list to enum list safely
                var statusEnums = approvalStatuses
                    .Where(s => Enum.TryParse(typeof(ApprovalStatus), s, true, out _))
                    .Select(s => (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), s, true))
                    .ToList();

                List<BulkUploadDetail> materialBulkUploadList = new List<BulkUploadDetail>();
                if (statusEnums.Contains(ApprovalStatus.None))
                {
                    materialBulkUploadList = await _bulkUploadDetailRepo.Query()
                    .Where(m => m.CreatedBy == currentUserEmail
                        && m.ApprovalStatus==null)
                        .ToListAsync();
                }
                else
                {
                     materialBulkUploadList = await _bulkUploadDetailRepo.Query()
                    .Where(m => m.ActionRequiredByUserEmail == currentUserEmail
                        && approvalStatuses.Contains(m.ApprovalStatus))
                        .ToListAsync();
                }

                bulkUploadDetailsDtoList = materialBulkUploadList.Select(bm => new BulkUploadDetailsDto
                {
                    Id = bm.Id,
                    NoOfRecords = bm.NoOfRecords,
                    FileName = bm.FileName,
                    FilePath = bm.FilePath,
                    Comment = bm.Comment,
                    ApprovalStatus = bm.ApprovalStatus,
                    ChangedBy = bm.UpdatedBy,
                    ChangedDate = bm.UpdatedDate

                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return bulkUploadDetailsDtoList;
        }
        
        //Private methods to this class
        private async Task<bool> AddApprovalStatusHistory(ApprovalStatusHistoryDto approvalStatusHistoryDto, string actionRequiredByUserEmail)
        {
            bool isSuccess = true;
            var approvalStatusHistory = new ApprovalStatusHistory()
            {
                StatusChangeBy = approvalStatusHistoryDto.StatusChangeBy,
                StatusChangeDate = approvalStatusHistoryDto.StatusChangeDate,
                ApprovalStatus = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), approvalStatusHistoryDto.ApprovalStatus),
                MaterialId = approvalStatusHistoryDto.MaterialId,
                Comments = approvalStatusHistoryDto.Comments,
                ActionRequiredByUserEmail = actionRequiredByUserEmail
            };
            await _genericRepository.Add(approvalStatusHistory);
            isSuccess = await UpdateMaterialCurrentApprovalStatus(approvalStatusHistoryDto.MaterialId, approvalStatusHistoryDto.ApprovalStatus.ToString());
            return isSuccess;
        }
        private async Task<bool> UpdateMaterialCurrentApprovalStatus(Guid materialId,string approvalStatus)
        {
            bool isSuccess = true;
            try
            {
                var existingMaterial = await _materialRepo.GetById(materialId);
                if (existingMaterial != null)
                {
                    existingMaterial.CurrentApprovalStatus = approvalStatus;
                    await _materialRepo.Update(existingMaterial);
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return isSuccess;
        }
        private async Task<bool> UpdateBulkUploadDetailsStatus(BulkApprovalInformationDto bulkApprovalInformationDto, string userEmail)
        {
            bool isSuccess = true;
            try
            {
                var existingBulkUploadDetail= await _bulkUploadDetailRepo.GetById(bulkApprovalInformationDto.BulkUploadDetailId);
                if (existingBulkUploadDetail != null)
                {
                    existingBulkUploadDetail.ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus;
                    existingBulkUploadDetail.UpdatedBy = bulkApprovalInformationDto.CurrentUserEmailId;
                    existingBulkUploadDetail.UpdatedDate = DateTime.Now;
                    existingBulkUploadDetail.ActionRequiredByUserEmail = userEmail;
                    await _bulkUploadDetailRepo.Update(existingBulkUploadDetail);
                }
            }
            catch(Exception ex)
            {
                isSuccess = false;
            }
           
            return isSuccess;
        }
        
        private async Task<bool> AddBulkReturnRejectStatus(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            var bulkUploadedMaterialslIds = await _materialRepo.Query()
                       .Where(h => h.BuilkUploadDetailId == bulkApprovalInformationDto.BulkUploadDetailId
                           && h.CurrentApprovalStatus.Equals(ApprovalStatus.Submmitted.ToString())
                           || h.CurrentApprovalStatus.Equals(ApprovalStatus.Reviewed.ToString())     )
                       .Select(m => m.Id)
                       .ToListAsync();

            foreach (var materialId in bulkUploadedMaterialslIds)
            {
                var existngSubmittedmaterial = await _genericRepository.Query()
                .Where(h => h.MaterialId == materialId
                    && h.ApprovalStatus == ApprovalStatus.Submmitted
                           || h.ApprovalStatus == ApprovalStatus.Reviewed)
                .OrderByDescending(h => h.StatusChangeDate)
                .FirstOrDefaultAsync();
               
                var approvalStatusHistoryDto = new ApprovalStatusHistoryDto
                {
                    MaterialId = materialId,
                    ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus,
                    StatusChangeBy = bulkApprovalInformationDto.CurrentUserEmailId,
                    StatusChangeDate = DateTime.Now,
                    Comments = bulkApprovalInformationDto.Comment,
                };
                await AddApprovalStatusHistory(approvalStatusHistoryDto, existngSubmittedmaterial.CreatedBy);
            }
            return true;
        }
        private async Task<bool> AddBulkApprovedStatus(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            var bulkUploadedMaterialslIds = await _materialRepo.Query()
                        .Where(h => h.BuilkUploadDetailId == bulkApprovalInformationDto.BulkUploadDetailId
                            && h.CurrentApprovalStatus == ApprovalStatus.Reviewed.ToString())
                        .Select(m => m.Id)
                        .ToListAsync();
            foreach (var materialId in bulkUploadedMaterialslIds)
            {
                var approvalStatusHistoryDto = new ApprovalStatusHistoryDto
                {
                    MaterialId = materialId,
                    ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus,
                    StatusChangeBy = bulkApprovalInformationDto.CurrentUserEmailId,
                    StatusChangeDate = DateTime.Now,
                    Comments = bulkApprovalInformationDto.Comment,
                };
                await AddApprovalStatusHistory(approvalStatusHistoryDto, null);
            }

            return true;
        }
        private async Task<bool> AddBulkReviewedStatus(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            var bulkUploadedMaterialslIds = await _materialRepo.Query()
                        .Where(h => h.BuilkUploadDetailId == bulkApprovalInformationDto.BulkUploadDetailId
                            && h.CurrentApprovalStatus == ApprovalStatus.Submmitted.ToString())
                        .Select(m => m.Id)
                        .ToListAsync();
            foreach (string userEmail in bulkApprovalInformationDto.ReviewerApproverEmailIds)
            {
                foreach (var materialId in bulkUploadedMaterialslIds)
                {
                    var approvalStatusHistoryDto = new ApprovalStatusHistoryDto
                    {
                        MaterialId = materialId,
                        ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus,
                        StatusChangeBy = bulkApprovalInformationDto.CurrentUserEmailId,
                        StatusChangeDate = DateTime.Now,
                        Comments = bulkApprovalInformationDto.Comment,
                    };
                    await AddApprovalStatusHistory(approvalStatusHistoryDto, userEmail);
                }
                actionRequiredByUserEmail = userEmail;
            }
            return true;
        }
    }
}
