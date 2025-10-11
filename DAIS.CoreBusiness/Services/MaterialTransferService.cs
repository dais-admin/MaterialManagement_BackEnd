﻿﻿﻿﻿﻿﻿﻿﻿﻿using AutoMapper;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using DAIS.DataAccess.Repositories;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialTransferService: IMaterialTransferService
    {
        private readonly IGenericRepository<MaterialIssueRecieveVoucher> _issueRecieveVoucherRepository;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<MaterialVoucherTransactionApproval> _trancationApproval;
        private readonly ILocationOperationService _locationOperationService;
        private readonly ILogger<MaterialTransferService> _logger;
        private readonly IMapper _mapper;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;
        private readonly IEmailService _mailService;
        private readonly IUserService _userService;
        private readonly ISubDivisionService _subDivisionService;
        private readonly IDivisionMaterialTransferService _divisionMaterialTransferService;
        private readonly ISubDivisionMaterialTransferService _subDivisionMaterialTransferService;

        private string userName = string.Empty;
        private string roleName = string.Empty;
        public MaterialTransferService(
            IGenericRepository<MaterialIssueRecieveVoucher> issueRecieveVoucherRepository,
            IGenericRepository<Material> material,
            ILocationOperationService locationOperationService,
            IGenericRepository<MaterialVoucherTransactionApproval> trancationApproval,
            ILogger<MaterialTransferService> logger, IMapper mapper,
            MaterialServiceInfrastructure materialServiceInfrastructure,
            IUserService userService, IEmailService mailService,
            ISubDivisionService subDivisionService,
            IDivisionMaterialTransferService divisionMaterialTransferService,
            ISubDivisionMaterialTransferService subDivisionMaterialTransferService
           )
        {
            _issueRecieveVoucherRepository = issueRecieveVoucherRepository;
            _material = material;
            _logger = logger;
            _mapper = mapper;
            _trancationApproval = trancationApproval;
            _locationOperationService = locationOperationService;
            _materialServiceInfrastructure = materialServiceInfrastructure;
            _userService = userService;
            _mailService = mailService;
            _subDivisionService = subDivisionService;
            _divisionMaterialTransferService = divisionMaterialTransferService;
            _subDivisionMaterialTransferService = subDivisionMaterialTransferService;
            if (_materialServiceInfrastructure.HttpContextAccessor.HttpContext != null)
            {
                SetUserAndProject(_materialServiceInfrastructure.HttpContextAccessor.HttpContext.User);
            }
        }
        private void SetUserAndProject(ClaimsPrincipal user)
        {
            try
            {
                if (user != null)
                {
                    userName = user.Claims.FirstOrDefault(x => x.Type == Claims.NameClaim).Value;

                    roleName = user.Claims.FirstOrDefault(x => x.Type == Claims.RoleClaim).Value;
                }
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<List<LocationMaterialTransferDto>> GetAllMaterialIssueRecieveVoucherByStatus(ApprovalStatus status)
        {
            _logger.LogInformation("MaterialTransferService:GetAllMaterialIssueRecieveVoucherByStatus:Method Start");
            
            try
            {
                // Get all voucher IDs with the specified status from MaterialVoucherTrancationApproval
                var voucherIdsWithStatus = await _trancationApproval.Query()
                    .Where(a => a.ApprovalStatus == status)
                    .Select(a => a.MaterialIssueRecieveVoucherId)
                    .Distinct()
                    .ToListAsync();
                List<MaterialIssueRecieveVoucher> issueRecieveVouchersWithStatus = null;
                // Get the vouchers with the specified status
                if(ApprovalStatus.Submmitted== status)
                {
                    issueRecieveVouchersWithStatus = await _issueRecieveVoucherRepository.Query()
                    .Where(v => voucherIdsWithStatus.Contains(v.Id))
                    .Include(x => x.IssueLocation)
                    .Include(x => x.RecieveLocation)
                    .Include(x => x.OnBoardedLocation)
                    .Include(x => x.Material)
                    .ToListAsync();
                }
                else
                {
                    issueRecieveVouchersWithStatus = await _issueRecieveVoucherRepository.Query()               
                    .Include(x => x.IssueLocation)
                    .Include(x => x.RecieveLocation)
                    .Include(x => x.OnBoardedLocation)
                    .Include(x => x.Material)
                    .ToListAsync();
                }
               
                // Map directly to LocationMaterialTransferDto
                var result = issueRecieveVouchersWithStatus.Select(v => new LocationMaterialTransferDto
                {
                    VoucherNo = v.VoucherNo,
                    Date = v.VoucherDate,
                    
                    IssuingLocationOperation = _mapper.Map<LocationOperationDto>(v.IssueLocation),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    OnBoardedQuantity = v.OnBoardedQuantity,
                    ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(v.RecieveLocation),
                    OnBoardedLocationOperation = v.OnBoardedLocation != null ? _mapper.Map<LocationOperationDto>(v.OnBoardedLocation) : null,
                    Material = _mapper.Map<MaterialDto>(v.Material),
                    ApprovalStatus = status
                }).ToList();
                
                _logger.LogInformation("MaterialTransferService:GetAllMaterialIssueRecieveVoucherByStatus:Method End");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllMaterialIssueRecieveVoucherByStatus");
                throw;
            }
        }
        public async Task<List<LocationMaterialTransferDto>> GetLocationMaterialTransfersByIssuingLocation(Guid locationId)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialTransfersByIssuingSubDivision:Method Start");

            try
            {
                // Get all transfers where the specified subdivision is the issuing subdivision
                var transfers = await _issueRecieveVoucherRepository.Query()
                    .Where(roleName == "MaterialIssuer" || roleName == "ExecutiveEngineer" ? x => x.IssueLocationId == locationId : x => x.RecieveLocationId == locationId)
                    .Include(x => x.IssueLocation)
                    .Include(x => x.RecieveLocation)
                    .Include(x => x.OnBoardedLocation)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();

                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for Location ID: {locationId}");
                    return new List<LocationMaterialTransferDto>();
                }

                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _trancationApproval.Query()
                    .Where(a => transferIds.Contains(a.MaterialIssueRecieveVoucherId))
                    .ToListAsync();

                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.MaterialIssueRecieveVoucherId, a => a);

                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);

                var result = new List<LocationMaterialTransferDto>();

                foreach (var group in groupedTransfers)
                {
                    var voucherNo = group.Key;
                    var transfersInGroup = group.ToList();
                    var firstTransfer = transfersInGroup.First();

                    // Check if there's an approval record for this transfer
                    ApprovalStatus? approvalStatus = null;
                    if (approvalLookup.TryGetValue(firstTransfer.Id, out var approval))
                    {
                        approvalStatus = approval.ApprovalStatus;
                    }

                    var dto = new LocationMaterialTransferDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingLocationOperation = _mapper.Map<LocationOperationDto>(firstTransfer.IssueLocation),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(firstTransfer.RecieveLocation),
                        OnBoardedLocationOperation = firstTransfer.OnBoardedLocation != null ? _mapper.Map<LocationOperationDto>(firstTransfer.OnBoardedLocation) : null,
                        Material = _mapper.Map<MaterialDto>(firstTransfer.Material),
                        ApprovalStatus = approvalStatus
                    };
                    if (roleName == "MaterialReciever" && approvalStatus == ApprovalStatus.Submmitted)
                    {
                        result.Add(dto);
                    }
                    if (roleName == "MaterialIssuer" && approvalStatus != ApprovalStatus.Approved)
                    {
                        result.Add(dto);
                    }
                    if (roleName == "ExecutiveEngineer")
                    {
                        result.Add(dto);
                    }
                }

                _logger.LogInformation($"Found {result.Count} transfer vouchers for Location ID: {locationId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving transfers for Location ID: {locationId}");
                throw;
            }
        }

        public async Task<MaterialTransferApprovalResponseDto> AddMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalDto)
        {
            _logger.LogInformation("MaterialTransferService:AddMaterialTransferApproval:Method Start");
            MaterialTransferApprovalResponseDto materialTransferApprovalResponseDto = new MaterialTransferApprovalResponseDto();
            try
            { 
                var issueRecieveVoucher = await _issueRecieveVoucherRepository.Query()
                                            .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalDto.VoucherNo);
                if(issueRecieveVoucher != null ) 
                {
                    var voucherTrancationApproval = new MaterialVoucherTransactionApproval()
                    {
                        MaterialIssueRecieveVoucherId = issueRecieveVoucher.Id,
                        ApprovalStatus = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), materialTransferApprovalDto.ApprovalStatus),
                        IssuerId = materialTransferApprovalDto.CurrentUserId,
                        RecieverId = materialTransferApprovalDto.ReviewerApproverId,
                        CreatedDate = DateTime.UtcNow,
                    };
                    var dbEntity = await _trancationApproval.Add(voucherTrancationApproval);
                    materialTransferApprovalResponseDto.IsSucess = true;
                    materialTransferApprovalResponseDto.Message = "Material Transfer sent for review";

                    var emailSubject = "Material Transfer for Approval";
                    var emailBody = "Hi \nMaterials are transfered from Location with Voucher No:" + materialTransferApprovalDto.VoucherNo +
                        "Please login into system to accept or Reject it.";
                    await SendEmail(materialTransferApprovalDto.ReviewerApproverId,
                        emailSubject, emailBody);
                }                                                              
                

            }
            catch (Exception ex)
            {
                materialTransferApprovalResponseDto.IsSucess = true;
                materialTransferApprovalResponseDto.Message = ex.Message;
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("MaterialTransferService:AddMaterialTransferApproval:Method End");
            return materialTransferApprovalResponseDto;
        }
        public async Task<bool> UpdateLocationMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            // Get the LocationMaterialTransfer by voucher number
            var locationMaterialTransfer = await _issueRecieveVoucherRepository.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (locationMaterialTransfer == null)
            {
                _logger.LogWarning($"No locationMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }
            // Check if there's already an approval record for this voucher
            var existingApproval = await _trancationApproval.Query()
                .FirstOrDefaultAsync(x => x.MaterialIssueRecieveVoucherId == locationMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }
            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate = DateTime.UtcNow;
            await _trancationApproval.Update(existingApproval);

            var emailSubject = "Material Transfer Status Updated";
            var emailBody = "Hi \nMaterials are tramsfered to division is updated with status:" + approvalStatus + "\nVoucher No:" + voucherNo +
                "Please login into system to check the status.";
            await SendEmail(existingApproval.IssuerId,
                emailSubject, emailBody);

            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
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

        public async Task<UserMaterialTransferDto> GetUserMaterialTransfers(string userId)
        {
            _logger.LogInformation("MaterialTransferService:GetUserMaterialTransfers:Method Start");
            
            try
            {
                // Create the result DTO
                var result = new UserMaterialTransferDto();
                
                // Get the user
                var user = await _userService.GetUserById(Guid.Parse(userId));
                if (user == null || !user.DivisionId.HasValue)
                {
                    _logger.LogWarning($"User not found or user has no division: {userId}");
                    return result;
                }
                
                // Get the user's division
                var divisionId = user.DivisionId.Value;
                
                // Get division material transfers
                result.DivisionMaterialTransfers = await _divisionMaterialTransferService.GetDivisionMaterialTransfersByIssuingDivision(divisionId);
                
                // Get all subdivisions under the user's division
                var subdivisions = await _subDivisionService.GetAllSubDivisionsByDivision(divisionId);
                
                // Get all material transfers for each subdivision
                foreach (var subdivision in subdivisions)
                {
                    var subDivisionTransfers = await _subDivisionMaterialTransferService.GetSubDivisionMaterialTransfersByIssuingSubDivision(subdivision.Id);
                    result.SubDivisionMaterialTransfers.AddRange(subDivisionTransfers);
                    
                    // Get all locations under this subdivision
                    var locations = await _locationOperationService.GetLocationsBySubDivisionId(subdivision.Id);
                    
                    // Get all material transfers for each location
                    foreach (var location in locations)
                    {
                        var locationTransfers = await GetLocationMaterialTransfersByIssuingLocation(location.Id);
                        result.LocationMaterialTransfers.AddRange(locationTransfers);
                    }
                }
                
                _logger.LogInformation("MaterialTransferService:GetUserMaterialTransfers:Method End");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserMaterialTransfers");
                throw;
            }
        }

    }
}
