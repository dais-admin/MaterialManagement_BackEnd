using AutoMapper;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialApprovalService : IMaterialApprovalService
    {
        private readonly IGenericRepository<MaterialApproval> _genericRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialApprovalService> _logger;
        private readonly IUserService _userService;
        private readonly IMaterialService _materialService;
        private readonly IBulkUploadDetailService _bulkUploadDetailService;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;
        private readonly IEmailService _mailService;
        private readonly MailSettings _mailSettings;

        private string userName = string.Empty;
        private Guid projectId = Guid.Empty;
        private string roleName = string.Empty;
        public MaterialApprovalService(IGenericRepository<MaterialApproval> genericRepository,
            IMapper mapper,
            ILogger<MaterialApprovalService> logger,
            IOptions<MailSettings> mailSettings,
            IEmailService mailService,
            IUserService userService,
            IMaterialService materialService,
            IBulkUploadDetailService bulkUploadDetailService,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _mailService = mailService;
            _mailSettings = mailSettings.Value;
            _materialService = materialService;
            _bulkUploadDetailService = bulkUploadDetailService;
            _materialServiceInfrastructure = materialServiceInfrastructure;

            if (_materialServiceInfrastructure.HttpContextAccessor.HttpContext != null)
            {
                SetUserAndProject(_materialServiceInfrastructure.HttpContextAccessor.HttpContext.User);
            }
        }
        private void SetUserAndProject(ClaimsPrincipal user)
        {
            if (user != null)
            {
                userName = user.Claims.FirstOrDefault(x => x.Type == Claims.NameClaim).Value;

                if (user.Claims.FirstOrDefault(x => x.Type == "ProjectId").Value != "")
                {
                    projectId = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "ProjectId").Value);
                }

                roleName = user.Claims.FirstOrDefault(x => x.Type == Claims.RoleClaim).Value;
            }
        }
        public async Task<MaterialApprovalDto> AddMaterialApproval(ApprovalInformationDto approvalInformationDto)
        {
            _logger.LogInformation("MaterialApprovalService:AddMaterialApproval:Method Start");
            MaterialApprovalDto materialApprovalDto = new MaterialApprovalDto();
            try
            {
                foreach (var reviewerApproverId in approvalInformationDto.ReviewerApproverIds)
                {
                    materialApprovalDto.ApprovalStatus =
                    (ApprovalStatus)approvalInformationDto.ApprovalStatus;
                    materialApprovalDto.MaterialId = approvalInformationDto.MaterialId;
                    materialApprovalDto.SubmitterId = approvalInformationDto.CurrentUserId;
                    materialApprovalDto.ReveiwerId = reviewerApproverId;
                    materialApprovalDto.CreatedDate =TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                    materialApprovalDto.IsActive = true;
                    var materialApproval = _mapper.Map<MaterialApproval>(materialApprovalDto);
                    var dbEntity = await _genericRepository.Add(materialApproval);
                    if (dbEntity != null)
                    {
                        await UpdateLastRejectedMaterialApproval(approvalInformationDto.MaterialId);
                        await SendEmailOnApprovalUpdate(dbEntity.Id, reviewerApproverId);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:AddMaterialApproval:Method End");
            return materialApprovalDto;
        }

        public async Task<MaterialApprovalDto> AddMaterialBulkApproval(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            _logger.LogInformation("MaterialApprovalService:AddMaterialApproval:Method Start");
            MaterialApprovalDto materialApprovalDto = new MaterialApprovalDto();
            try
            {
                foreach (var reviewerApproverId in bulkApprovalInformationDto.ReviewerApproverIds)
                {
                    materialApprovalDto.ApprovalStatus =
                    (ApprovalStatus)bulkApprovalInformationDto.ApprovalStatus;                  
                    materialApprovalDto.SubmitterId = bulkApprovalInformationDto.CurrentUserId;
                    materialApprovalDto.ReveiwerId = reviewerApproverId;
                    materialApprovalDto.CreatedDate = DateTime.UtcNow.AddHours(5.5);
                    materialApprovalDto.IsActive = true;
                    materialApprovalDto.IsActive = true;

                    var materialIds = await _materialService.GetAllMaterialIdsByBulkUploadIdAsync(bulkApprovalInformationDto.BulkUploadDetailId);
                    foreach( Guid materialId in materialIds)
                    {
                        materialApprovalDto.MaterialId = materialId;
                        var materialApproval = _mapper.Map<MaterialApproval>(materialApprovalDto);
                        var dbEntity = await _genericRepository.Add(materialApproval);
                    }

                    await SendEmailOnBulkApprovalUpdate(materialApprovalDto.ApprovalStatus, reviewerApproverId);
                }
                await _bulkUploadDetailService.UpdateBulkUpload(
                    new BulkUploadDetailsDto()
                    {
                        Id = bulkApprovalInformationDto.BulkUploadDetailId,
                        ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus,
                        Comment = bulkApprovalInformationDto.Comment,
                    });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:AddMaterialApproval:Method End");
            return materialApprovalDto;
        }

        public async Task UpdateLastRejectedMaterialApproval(Guid materialId)
        {
            var existingMaterialApprovalList= await _genericRepository.Query()
                    .Where(x => x.MaterialId == materialId && x.IsActive == true)
                    .Include(x => x.Material)
                    .Include(x => x.Reveiwer)
                    .Include(x => x.Approver)
                    .ToListAsync().ConfigureAwait(false);
            foreach (var existingMaterialApproval in existingMaterialApprovalList)
            {
                if (existingMaterialApproval.ApprovalStatus == ApprovalStatus.ReviewerRejected ||
                existingMaterialApproval.ApprovalStatus == ApprovalStatus.ApproverRejected)
                {
                    existingMaterialApproval.IsActive = false;
                    await _genericRepository.Update(existingMaterialApproval);
                }
            }

        }
        public async Task DeleteMaterialApproval(Guid id)
        {
            _logger.LogInformation("MaterialApprovalService:DeleteMaterialApproval:Method Start");

            try
            {
                var materialApproval = await _genericRepository.GetById(id);
                await _genericRepository.Remove(materialApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:DeleteMaterialApproval:Method End");
        }

        public async Task<List<MaterialApprovalDto>> GetAllMaterialApprovals(Guid workPackageId)
        {
            _logger.LogInformation("MaterialApprovalService:GetAllMaterialApprovals:Method Start");
            List<MaterialApprovalDto> materialApprovalDtoList = new List<MaterialApprovalDto>();
            try
            {
                var materialApprovalList = await _genericRepository.Query()
                    .Where(x=>x.Material.WorkPackageId == workPackageId)
                    .Include(x => x.Material)
                    .Include(x => x.Reveiwer)
                    .Include(x => x.Approver)
                    .Include(x=>x.Material.WorkPackage)
                    .ToListAsync().ConfigureAwait(false);

                var materialApprovalDto = _mapper.Map<List<MaterialApprovalDto>>(materialApprovalList);
                materialApprovalDtoList.AddRange(materialApprovalDto.ToList().DistinctBy(x=>x.MaterialId));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:GetAllMaterialApprovals:Method End");
            return materialApprovalDtoList;
        }

        public async Task<MaterialApprovalDto> GetMaterialApproval(Guid id)
        {
            _logger.LogInformation("MaterialApprovalService:GetMaterialApproval:Method Start");
            MaterialApprovalDto materialApprovalDto = new MaterialApprovalDto();
            try
            {
                var materialApproval = await _genericRepository.Query()
                    .Include(x => x.Material)
                    .Include(x => x.Reveiwer)
                    .Include(x => x.Approver)
                    .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                materialApprovalDto = _mapper.Map<MaterialApprovalDto>(materialApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:GetMaterialApproval:Method End");
            return materialApprovalDto;
        }

        public async Task<MaterialApprovalDto> UpdateMaterialApproval(MaterialApprovalDto materialApprovalDto)
        {
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialApproval:Method Start");

            try
            {
                var materialApproval = _mapper.Map<MaterialApproval>(materialApprovalDto);
                await _genericRepository.Update(materialApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialApproval:Method End");
            return materialApprovalDto;
        }
        public async Task<MaterialApprovalDto> UpdateMaterialApprovalStatus(ApprovalInformationDto approvalInformationDto)
        {
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialApprovalStatus:Method Start");

            MaterialApprovalDto materialApprovalDto = new MaterialApprovalDto();
            try
            {
                var materialApprovals = await _genericRepository.Query()
                    .Where(x => x.MaterialId == approvalInformationDto.MaterialId).ToListAsync();
                string userIdForEmail = string.Empty;
                foreach (var materialApproval in materialApprovals)
                {
                    materialApproval.ApprovalStatus =
                    (ApprovalStatus)approvalInformationDto.ApprovalStatus;

                    switch (materialApproval.ApprovalStatus)
                    {
                        case ApprovalStatus.Reviewed:
                            materialApproval.ApproverId = approvalInformationDto.ReviewerApproverIds.FirstOrDefault();
                            materialApproval.ReviewedDate =  TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); 
                            materialApprovalDto.IsActive = true;
                            materialApproval.ReviewerComment = approvalInformationDto.Comment;
                            userIdForEmail = approvalInformationDto.ReviewerApproverIds.FirstOrDefault();
                            break;

                        case ApprovalStatus.Approved:
                            materialApproval.ApprovedDate =  TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                            materialApprovalDto.IsActive = true;
                            materialApproval.ApproverComment = approvalInformationDto.Comment;
                            userIdForEmail = materialApproval.SubmitterId;
                            break;
                        case ApprovalStatus.ReviewerReturned:
                            materialApproval.ReviewerComment = approvalInformationDto.Comment;
                            materialApproval.ReviewedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                            materialApprovalDto.IsActive = true;
                            userIdForEmail = materialApproval.SubmitterId;
                            break;
                        case ApprovalStatus.ReviewerRejected:
                            materialApproval.ReviewerComment = approvalInformationDto.Comment;
                            materialApproval.ReviewedDate =  TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                            materialApprovalDto.IsActive = true;
                            userIdForEmail = materialApproval.SubmitterId;
                            break;
                        case ApprovalStatus.ApproverReturened:
                            materialApproval.ApproverComment = approvalInformationDto.Comment;
                            materialApproval.ApprovedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                            materialApprovalDto.IsActive = true;
                            userIdForEmail = materialApproval.SubmitterId;
                            break;
                        case ApprovalStatus.ApproverRejected:
                            materialApproval.ApproverComment = approvalInformationDto.Comment;
                            materialApproval.ApprovedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); ;
                            materialApprovalDto.IsActive = true;
                            userIdForEmail = materialApproval.SubmitterId;
                            break;

                        default:
                            _logger.LogWarning("Unknown approval status: {ApprovalStatus}", materialApproval.ApprovalStatus);
                            break;
                    }

                    await _genericRepository.Update(materialApproval).ConfigureAwait(false);

                    await SendEmailOnApprovalUpdate(materialApproval.Id, userIdForEmail);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialApprovalStatus:Method End");
            return materialApprovalDto;
        }

        public async Task<BulkApprovalResponseDto> UpdateMaterialBulkApprovalStatus(BulkApprovalInformationDto bulkApprovalInformationDto)
        {
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialBulkApprovalStatus:Method Start");

            BulkApprovalResponseDto bulkApprovalResponseDto = new BulkApprovalResponseDto();
            try
            {
                var materialIds = await _materialService.GetAllMaterialIdsByBulkUploadIdAsync(bulkApprovalInformationDto.BulkUploadDetailId);
                foreach (Guid materialId in materialIds)
                {
                    
                    var materialApprovals = await _genericRepository.Query()
                        .Where(x => x.MaterialId == materialId).ToListAsync();
                    string userIdForEmail = string.Empty;
                    foreach (var materialApproval in materialApprovals)
                    {
                        materialApproval.ApprovalStatus =
                        (ApprovalStatus)bulkApprovalInformationDto.ApprovalStatus;

                        switch (materialApproval.ApprovalStatus)
                        {
                            case ApprovalStatus.Reviewed:
                                materialApproval.ApproverId = bulkApprovalInformationDto.ReviewerApproverIds.FirstOrDefault();
                                materialApproval.ReviewedDate = DateTime.Now;
                                materialApproval.ReviewerComment = bulkApprovalInformationDto.Comment;
                                userIdForEmail = bulkApprovalInformationDto.ReviewerApproverIds.FirstOrDefault();
                                break;

                            case ApprovalStatus.Approved:
                                materialApproval.ApprovedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                                materialApproval.ApproverComment = bulkApprovalInformationDto.Comment;
                                userIdForEmail = materialApproval.SubmitterId;
                                break;
                            case ApprovalStatus.ReviewerReturned:
                                materialApproval.ReviewerComment = bulkApprovalInformationDto.Comment;
                                materialApproval.ReviewedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                                userIdForEmail = materialApproval.SubmitterId;
                                break;
                            case ApprovalStatus.ReviewerRejected:
                                materialApproval.ReviewerComment = bulkApprovalInformationDto.Comment;
                                materialApproval.ReviewedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); 
                                userIdForEmail = materialApproval.SubmitterId;
                                break;
                            case ApprovalStatus.ApproverReturened:
                                materialApproval.ApproverComment = bulkApprovalInformationDto.Comment;
                                materialApproval.ApprovedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); 
                                userIdForEmail = materialApproval.SubmitterId;
                                break;
                            case ApprovalStatus.ApproverRejected:
                                materialApproval.ApproverComment = bulkApprovalInformationDto.Comment;
                                materialApproval.ApprovedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")); 
                                userIdForEmail = materialApproval.SubmitterId;
                                break;

                            default:
                                _logger.LogWarning("Unknown approval status: {ApprovalStatus}", materialApproval.ApprovalStatus);
                                break;
                        }

                        await _genericRepository.Update(materialApproval).ConfigureAwait(false);

                        //await SendEmailOnApprovalUpdate(materialApproval.Id, userIdForEmail);

                    }
                }
                await _bulkUploadDetailService.UpdateBulkUpload(
                    new BulkUploadDetailsDto()
                    {
                        Id = bulkApprovalInformationDto.BulkUploadDetailId,
                        ApprovalStatus = bulkApprovalInformationDto.ApprovalStatus,
                        Comment = bulkApprovalInformationDto.Comment,
                    });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:UpdateMaterialBulkApprovalStatus:Method End");
            return bulkApprovalResponseDto;
        }


        public async Task<List<MaterialApprovalDto>> GetMaterialsByStatusAsync(ApprovalStatus approvalStatus,bool isActive, string userId)
        {
            _logger.LogInformation("MaterialApprovalService:GetMaterialsByStatusAsync:Method Start");
            try
            {
                return await GetMaterialsByStatusForUserAsync(approvalStatus, isActive, userId).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }
        private async Task<List<MaterialApprovalDto>> GetMaterialsByStatusForUserAsync(ApprovalStatus approvalStatus, bool isActive, string userId)
        {
            _logger.LogInformation($"MaterialApprovalService:GetMaterialsByStatusForUserAsync:Method Start for {isActive}");
            List<MaterialApprovalDto> materialApprovalDtoList = new List<MaterialApprovalDto>();
            try
            {
                IQueryable<MaterialApproval> query = _genericRepository.Query()
                .Where(x=>x.IsActive==isActive)
                .Include(x => x.Material)
                .Include(x => x.Reveiwer)
                .Include(x => x.Approver);

                var filteredMaterialApprovalList= approvalStatus switch
                {
                    var status when status == ApprovalStatus.Submmitted => await query
                        .Where(x => x.SubmitterId == userId && x.ApprovalStatus==ApprovalStatus.Submmitted
                        || x.ApprovalStatus==ApprovalStatus.Reviewed)
                        .ToListAsync(),

                    var status when status == ApprovalStatus.Reviewed => await query
                        .Where(x => x.ReveiwerId == userId && x.ApprovalStatus==ApprovalStatus.Reviewed)
                        .ToListAsync(),
                    var status when status == ApprovalStatus.ReviewerRejected => await query
                        .Where(x => x.ReveiwerId == userId && x.ApprovalStatus==ApprovalStatus.ReviewerRejected)
                        .ToListAsync(),

                    var status when status == ApprovalStatus.Approved => await query
                        .Where(x => x.ApproverId == userId && x.ApprovalStatus==ApprovalStatus.Approved)
                    .ToListAsync(),

                    var status when status == ApprovalStatus.ApproverRejected => await query
                        .Where(x => x.ApproverId == userId && x.ApprovalStatus==ApprovalStatus.ApproverRejected)
                    .ToListAsync(),

                    var status when status == ApprovalStatus.None => await query                  
                   .ToListAsync(),

                    _ => throw new ArgumentException("Invalid Approval Status.")
                };
                materialApprovalDtoList = _mapper.Map<List<MaterialApprovalDto>>(filteredMaterialApprovalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving materials.");
                throw;
            }

            _logger.LogInformation($"MaterialApprovalService:GetMaterialsByStatusForUserAsync:Method End for {isActive}");

            return materialApprovalDtoList;
        }
        public async Task<MaterialApprovalDto> GetMaterialApproveByMaterialId(Guid materialId)
        {
            _logger.LogInformation("MaterialApprovalService:GetMaterialApproveByMaterialId:Method Start");
            MaterialApprovalDto materialApprovalDto = new MaterialApprovalDto();
            try
            {
                var materialApproval = await _genericRepository.Query()
                    .Where(x => x.MaterialId == materialId && x.IsActive == true)
                    .Include(x => x.Material)
                    .Include(x => x.Reveiwer)
                    .Include(x => x.Approver)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

                materialApprovalDto = _mapper.Map<MaterialApprovalDto>(materialApproval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:GetMaterialsForApprove:Method End");
            return materialApprovalDto;
        }
        public async Task<List<MaterialApprovalDto>> GetMaterialListByUserIdAsync(string userId, string userRole)
        {
            _logger.LogInformation("MaterialApprovalService:GetMaterialListByIdsAsync:Method Start");
            List<MaterialApprovalDto> materialApprovalDtoList = new List<MaterialApprovalDto>();
            try
            {
                materialApprovalDtoList = await GetMaterialListForUser(userId, userRole);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            _logger.LogInformation("MaterialApprovalService:GetMaterialListByIdsAsync:Method End");
            return materialApprovalDtoList;
        }
        public async Task<List<BulkApprovalResponseDto>> GetBulkApprovalMaterialListByUserId(string userId, string userRole)
        {
            _logger.LogInformation("MaterialApprovalService:GetBulkApprovalMaterialListByUserId:Method Start");
           
            List<BulkApprovalResponseDto> bulkApprovalResponseDtoList=new List<BulkApprovalResponseDto>();
            try
            {
                var materialApprovalList = await GetMaterialApprovalsAsync(userId, userRole).ConfigureAwait(false);
                materialApprovalList = materialApprovalList
                     .Where(x => x.IsActive == true && x.Material.BuilkUploadDetailId!=null)
                     .ToList().DistinctBy(x => x.Material.BuilkUploadDetailId).ToList();
                if (materialApprovalList.Any())
                {
                    foreach (var materialApproval in materialApprovalList)
                    {
                        BulkApprovalResponseDto bulkApprovalResponseDto = new BulkApprovalResponseDto();
                        bulkApprovalResponseDto.ApprovalStatus = materialApproval.ApprovalStatus;
                        bulkApprovalResponseDto.Submitter = _mapper.Map<UserDto>(materialApproval.Submitter);
                        bulkApprovalResponseDto.Reveiwer = _mapper.Map<UserDto>(materialApproval.Reveiwer);
                        bulkApprovalResponseDto.Approver = _mapper.Map<UserDto>(materialApproval.Approver);
                        bulkApprovalResponseDto.ReviewerComment = materialApproval.ReviewerComment;
                        bulkApprovalResponseDto.ApproverComment = materialApproval.ApproverComment;
                        var builUploadDetailId = materialApproval.Material.BuilkUploadDetailId;
                        bulkApprovalResponseDto.BulkUploadDetails = await _bulkUploadDetailService
                            .GetBulkUploadDetailById((Guid)builUploadDetailId);

                        bulkApprovalResponseDtoList.Add(bulkApprovalResponseDto);
                    }
                }
                else
                {
                    if (userRole == UserTypes.Submitter.ToString())
                    {
                        var materials = await _materialService.GetMaterialsAddedByCurrentUser();
                        List<Guid> bulkUploadIds = new List<Guid>();
                        foreach (var builUpload in  materials.GroupBy(x => x.BuilkUploadDetailId))
                        {
                            if (builUpload.Select(x => x.BuilkUploadDetailId).FirstOrDefault() != null)
                            {
                                bulkUploadIds.Add((Guid)builUpload.Select(x => x.BuilkUploadDetailId).FirstOrDefault());
                            }
                        }
                           
                        foreach ( var bulkUploadID in bulkUploadIds)
                        {
                            BulkApprovalResponseDto bulkApprovalResponseDto = new BulkApprovalResponseDto();
                            bulkApprovalResponseDto.ApprovalStatus = ApprovalStatus.None;
                            
                            bulkApprovalResponseDto.BulkUploadDetails = await _bulkUploadDetailService
                            .GetBulkUploadDetailById(bulkUploadID);

                            if (bulkApprovalResponseDto.BulkUploadDetails.ApprovalStatus == null)
                            {
                                bulkApprovalResponseDtoList.Add(bulkApprovalResponseDto);
                            }
                            
                        }
                        

                    }
                }
                
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:GetBulkApprovalMaterialListByUserId:Method End");
            return bulkApprovalResponseDtoList;
        }
        private async Task<List<MaterialApprovalDto>> GetMaterialListForUser(string userId, string userRole)
        {
            _logger.LogInformation("MaterialApprovalService:GetMaterialIdsForUser:Method Start");
            List<MaterialApprovalDto> materialApprovalDtoList = new List<MaterialApprovalDto>();
            try
            {
                var materialApprovalList = await GetMaterialApprovalsAsync(userId, userRole).ConfigureAwait(false);
                materialApprovalList = materialApprovalList
                     .Where(x => x.IsActive == true)                 
                     .ToList().DistinctBy(x => x.MaterialId).ToList();
                materialApprovalDtoList = _mapper.Map<List<MaterialApprovalDto>>(materialApprovalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:GetMaterialIdsForUser:Method End");
            return materialApprovalDtoList;
        }

        private async Task<List<MaterialApproval>> GetMaterialApprovalsAsync(string userId, string userRole)
        {
            //TODO: Need to handle admin role
            IQueryable<MaterialApproval> query = _genericRepository.Query()
                .Include(x => x.Material)
                .Include(x => x.Reveiwer)
                .Include(x => x.Approver);

            return userRole switch
            {
                var role when role == UserTypes.Reviewer.ToString() => await query
                    .Where(x => x.ReveiwerId == userId && x.ApprovalStatus == ApprovalStatus.Submmitted)
                    .ToListAsync(),

                var role when role == UserTypes.Approver.ToString() => await query
                    .Where(x => x.ApproverId == userId && x.ApprovalStatus == ApprovalStatus.Reviewed)
                    .ToListAsync(),

                var role when role == UserTypes.Submitter.ToString() => await query
                    .Where(x => x.SubmitterId == userId && x.ApprovalStatus == ApprovalStatus.ReviewerRejected
                    || x.ApprovalStatus == ApprovalStatus.ApproverRejected || x.ApprovalStatus==ApprovalStatus.ReviewerReturned
                    || x.ApprovalStatus==ApprovalStatus.ApproverReturened || x.ApprovalStatus==ApprovalStatus.Submmitted)
                    .ToListAsync(),

                var role when role == (UserTypes.Admin.ToString()) => await query
               .Where(x => x.ApprovalStatus == ApprovalStatus.ReviewerRejected
               || x.ApprovalStatus == ApprovalStatus.ApproverRejected || x.ApprovalStatus == ApprovalStatus.Reviewed || x.ApprovalStatus == ApprovalStatus.Submmitted)
               .ToListAsync(),

                _ => throw new ArgumentException("Invalid user role.")
            };
        }
        private async Task<bool> SendEmailOnBulkApprovalUpdate(ApprovalStatus approvalStatus, string userId)
        {
            bool isSucess = false;
            _logger.LogInformation("MaterialApprovalService:SendEmailOnBulkApprovalUpdate:Method Start");
            try
            {
                string emailSubject = string.Empty;
                string emailBody = string.Empty;
                switch (approvalStatus)
                {
                    case ApprovalStatus.Submmitted:
                        emailSubject = "Bulk Uploaded Material Submitted for Review";
                        emailBody = "Bulk Uploaded Material Information is Submitted for Review.\n" +
                            "Please login into system to review the all information. ";
                        isSucess = await SendEmail(userId, emailSubject, emailBody);
                        break;
                    case ApprovalStatus.Reviewed:
                        emailSubject = "Bulk Uploaded Material sent for Approval";
                        emailBody = "Bulk Uploaded Material is Submitted for Approval.\n" +
                            "\nPlease login into system to approve the all information. ";
                        isSucess = await SendEmail(userId, emailSubject, emailBody);
                        break;
                    case ApprovalStatus.Approved:
                        emailSubject = "Bulk Uploaded Material Information Approved";
                        emailBody = "Bulk Uploaded Material Information is Approved.\n" +
                            "\nPlease login into system to verify. ";
                        isSucess = await SendEmail(userId, emailSubject, emailBody);
                        break;
                    case ApprovalStatus.ReviewerRejected:
                        emailSubject = "Bulk Uploaded Material Information Rejected By Reviewer";
                        emailBody = "Bulk Uploaded Material Information is Rejected By Reviewer.\n" +
                            
                            "\nPlease login into system to update the information. ";

                        isSucess = await SendEmail(userId, emailSubject, emailBody);
                        break;
                    case ApprovalStatus.ApproverRejected:
                        emailSubject = "Bulk Uploaded Material Information Rejected By Approver";
                        emailBody = "Bulk Uploaded Material Information is Submitted for Approval.\n" +
                            "\nPlease login into system to update the information. ";
                        isSucess = await SendEmail(userId, emailSubject, emailBody);
                        break;
                    default:
                        break;


                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:SendEmailOnBulkApprovalUpdate:Method End");
            return isSucess;
        }
        private async Task<bool> SendEmailOnApprovalUpdate(Guid approvalId, string userId)
        {
            bool isSucess = false;
            _logger.LogInformation("MaterialApprovalService:SendEmailOnApprovalUpdate:Method Start");
            try
            {
                string emailSubject = string.Empty;
                string emailBody = string.Empty;
                var materialApprovalDto = await GetMaterialApproval(approvalId);

                if (materialApprovalDto != null)
                {
                    switch (materialApprovalDto.ApprovalStatus)
                    {
                        case ApprovalStatus.Submmitted:
                            emailSubject = "Material Information Submitted for Review";
                            emailBody = "Below Material Information is Submitted for Review.\n" +
                                "Material Code:" + materialApprovalDto.Material.MaterialCode + "\nMaterial Name: "
                                + materialApprovalDto.Material.MaterialName + "\nPlease login into system to review the all information. ";
                            isSucess = await SendEmail(userId, emailSubject, emailBody);
                            break;
                        case ApprovalStatus.Reviewed:
                            emailSubject = "Material Information sent for Approval";
                            emailBody = "Below Material Information is Submitted for Approval.\n" +
                                "Material Code:" + materialApprovalDto.Material.MaterialCode + "\nMaterial Name: "
                                + materialApprovalDto.Material.MaterialName + "\nPlease login into system to approve the all information. ";
                            isSucess = await SendEmail(userId, emailSubject, emailBody);
                            break;
                        case ApprovalStatus.Approved:
                            emailSubject = "Material Information Approved";
                            emailBody = "Below Material Information is Approved.\n" +
                                "Material Code:" + materialApprovalDto.Material.MaterialCode + "\nMaterial Name: "
                                + materialApprovalDto.Material.MaterialName + "\nPlease login into system to verify. ";
                            isSucess = await SendEmail(userId, emailSubject, emailBody);
                            break;
                        case ApprovalStatus.ReviewerRejected:
                            emailSubject = "Material Information Rejected By Reviewer";
                            emailBody = "Below Material Information is Rejected By Reviewer.\n" +
                                "Material Code:" + materialApprovalDto.Material.MaterialCode + "\nMaterial Name: "
                                + materialApprovalDto.Material.MaterialName + "\nComment :" + materialApprovalDto.ReviewerComment +
                                "\nPlease login into system to update the information. ";

                            isSucess = await SendEmail(userId, emailSubject, emailBody);
                            break;
                        case ApprovalStatus.ApproverRejected:
                            emailSubject = "Material Information Rejected By Approver";
                            emailBody = "Below Material Information is Submitted for Approval.\n" +
                                "Material Code:" + materialApprovalDto.Material.MaterialCode + "\nMaterial Name: "
                                + materialApprovalDto.Material.MaterialName + "\nComment :" + materialApprovalDto.ApproverComment +
                                "\nPlease login into system to update the information. ";
                            isSucess = await SendEmail(userId, emailSubject, emailBody);
                            break;
                        default:
                            break;


                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:SendEmailOnApprovalUpdate:Method End");
            return isSucess;
        }
        private async Task<bool> SendEmail(string userId, string emailSubject, string emailBody)
        {
            bool isSucess = false;
            _logger.LogInformation("MaterialApprovalService:SendEmailOnApprovalUpdate:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var users = await _userService.GetAllUsers();
                    var user = users.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefault();
                    if (user != null)
                    {
                        MailData mailData = new MailData()
                        {
                            RecipientEmail = user.Email,
                            RecipientName = user.UserName,
                            EmailSubject = emailSubject,
                            EmailBody = emailBody
                        };
                        isSucess = await _mailService.SendEmailAsync(mailData, null);

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialApprovalService:SendEmailOnApprovalUpdate:Method End");
            return isSucess;
        }
    }
}
