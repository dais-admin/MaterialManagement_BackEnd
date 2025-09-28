
using AutoMapper;
using DAIS.CoreBusiness.Dtos;
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
        private readonly IMapper _mapper;
        private readonly ILogger<ApprovalStatusHistoryService> _logger;
        public ApprovalStatusHistoryService(IGenericRepository<ApprovalStatusHistory> genericRepository,
            IMapper mapper,ILogger<ApprovalStatusHistoryService> logger, IGenericRepository<Material> materialRepo)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
            _materialRepo = materialRepo;
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
                    var approvalStatusHistory = new ApprovalStatusHistory()
                    {
                        StatusChangeBy = approvalStatusHistoryDto.StatusChangeBy,
                        StatusChangeDate = approvalStatusHistoryDto.StatusChangeDate,
                        ApprovalStatus = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), approvalStatusHistoryDto.ApprovalStatus),
                        MaterialId = approvalStatusHistoryDto.MaterialId,
                        Comments = approvalStatusHistoryDto.Comments,
                        ActionRequiredByUserEmail = existngSubmittedmaterial.StatusChangeBy
                    };
                    await _genericRepository.Add(approvalStatusHistory);

                }
                if (approvalStatusHistoryDto.ApprovalStatus.Equals(ApprovalStatus.Approved.ToString()))
                {
                    var approvalStatusHistory = new ApprovalStatusHistory()
                    {
                        StatusChangeBy = approvalStatusHistoryDto.StatusChangeBy,
                        StatusChangeDate = approvalStatusHistoryDto.StatusChangeDate,
                        ApprovalStatus = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), approvalStatusHistoryDto.ApprovalStatus),
                        MaterialId = approvalStatusHistoryDto.MaterialId,
                        Comments = approvalStatusHistoryDto.Comments,
                        ActionRequiredByUserEmail = null
                    };
                    await _genericRepository.Add(approvalStatusHistory);
                }
                if(approvalStatusHistoryDto.ReviewerApproverEmailIds!= null)
                {
                    foreach (string userEmail in approvalStatusHistoryDto.ReviewerApproverEmailIds)
                    {
                        var approvalStatusHistory = new ApprovalStatusHistory()
                        {
                            StatusChangeBy = approvalStatusHistoryDto.StatusChangeBy,
                            StatusChangeDate = approvalStatusHistoryDto.StatusChangeDate,
                            ApprovalStatus = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), approvalStatusHistoryDto.ApprovalStatus),
                            MaterialId = approvalStatusHistoryDto.MaterialId,
                            Comments = approvalStatusHistoryDto.Comments,
                            ActionRequiredByUserEmail = userEmail
                        };
                        await _genericRepository.Add(approvalStatusHistory);
                    }
                }
                
                isSuccess = await UpdateMaterialCurrentApprovalStatus(approvalStatusHistoryDto.MaterialId, approvalStatusHistoryDto.ApprovalStatus.ToString());
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
                    CurrentApprovalStatus = material.CurrentApprovalStatus,
                    ApprovalStatusHistory = material.ApprovalStatusHistory.Select(history => new ApprovalStatusHistoryDto
                    {
                        StatusChangeBy = history.StatusChangeBy,
                        StatusChangeDate = history.StatusChangeDate,
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
    }
}
