﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using AutoMapper;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class DivisionMaterialTransferService: IDivisionMaterialTransferService
    {
        private readonly IGenericRepository<DivisionMaterialTransfer> _divisionTransferRepo;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<DivisionMaterialTransferTrancation> _divisionTransferTrancationRepo;
        private readonly IGenericRepository<DivisionMaterialTransferApproval> _divisionTransferApprovalRepo;
        private readonly ILogger<DivisionMaterialTransferService> _logger;
        private readonly IDivisionService _divisionService;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly IUserService _userService;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private string roleName = string.Empty;
        public DivisionMaterialTransferService(IGenericRepository<DivisionMaterialTransfer> divisionTransferRepo,
            IGenericRepository<DivisionMaterialTransferTrancation> divisionTransferTrancationRepo,
            IGenericRepository<DivisionMaterialTransferApproval> divisionTransferApprovalRepo,
            IGenericRepository<Material> material, IMapper mapper,
            IDivisionService divisionService,
            ILogger<DivisionMaterialTransferService> logger,
            IUserService userService, IEmailService mailService,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _divisionTransferRepo = divisionTransferRepo;
            _divisionTransferTrancationRepo = divisionTransferTrancationRepo;
            _divisionTransferApprovalRepo = divisionTransferApprovalRepo;
            _material = material;
            _divisionService = divisionService;
            _mapper = mapper;
            _logger = logger;
            _materialServiceInfrastructure= materialServiceInfrastructure;
            _userService = userService;
            _mailService = mailService;

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
        public async Task<DivisionMaterialTransferDto> GetDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            _logger.LogInformation("DivisionMaterialTransferService:GetDivisionMaterialTransferByVoucherNo:Method Start");
            
            var voucherEntities = _divisionTransferRepo.Query()
                .Include(x => x.IssueDivision)
                .Include(x => x.RecieveDivision)
                .Include(x => x.OnBoardedDivision)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var divisionMaterialTransferDto = new DivisionMaterialTransferDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                DivisionMaterialTransferItems = voucherEntities.Select(v => new DivisionMaterialTransferItemDto
                {
                    IssuingDivisionId = v.IssueDivisionId,
                    IssuingDivision = _mapper.Map<DivisionDto>(v.IssueDivision),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    ReceivingDivisionId = v.RecieveDivisionId,
                    ReceivingDivision = _mapper.Map<DivisionDto>(v.RecieveDivision),
                    OnBoardedDivisionId = v.OnBoardedDivisionId,
                    OnBoardedDivision = v.OnBoardedDivision != null ? _mapper.Map<DivisionDto>(v.OnBoardedDivision) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            _logger.LogInformation("DivisionMaterialTransferService:GetDivisionMaterialTransferByVoucherNo:Method End");
            return divisionMaterialTransferDto;
        }
        public async Task<MaterialIssueReceiveResponseDto> AddDivisionMaterialTransfer(DivisionMaterialTransferDto divisionMaterialTransferDto)
        {
            _logger.LogInformation("DivisionMaterialTransferService:AddDivisionMaterialTransfer:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto =
                new MaterialIssueReceiveResponseDto();
            try
            {

                materialIssueReceiveResponseDto = await CheckMaterialValidForIssueRecive(divisionMaterialTransferDto.DivisionMaterialTransferItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }
                foreach (DivisionMaterialTransferItemDto divisionMaterialTransferItem in divisionMaterialTransferDto.DivisionMaterialTransferItems)
                {
                    var divisionMaterialTransferTrancations = new List<DivisionMaterialTransferTrancation>();


                    var checkStock = await _divisionTransferRepo.GetWhere(x => x.MaterialId == divisionMaterialTransferItem.MaterialId
                    && (x.IssueDivisionId == divisionMaterialTransferItem.IssuingDivisionId)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == divisionMaterialTransferItem.MaterialId
                    && x.DivisionId == divisionMaterialTransferItem.IssuingDivisionId);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var divisionMaterialTransfer = new DivisionMaterialTransfer()
                    {
                        VoucherNo = divisionMaterialTransferDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(divisionMaterialTransferDto.Date),
                        VoucherType = divisionMaterialTransferItem.VoucherType,
                        IssueDivisionId = divisionMaterialTransferItem.IssuingDivisionId,
                        RecieveDivisionId = divisionMaterialTransferItem.ReceivingDivisionId,
                        OnBoardedDivisionId = divisionMaterialTransferItem.OnBoardedDivisionId,
                        IssuedQuantity = divisionMaterialTransferItem.IssuedQuantity,
                        RecievedQuantity = divisionMaterialTransferItem.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = divisionMaterialTransferItem.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock

                    };

                    if (divisionMaterialTransferItem.VoucherType == VoucherType.Issue)
                    {

                        divisionMaterialTransfer.IssuedQuantity = divisionMaterialTransferItem.IssuedQuantity;
                        divisionMaterialTransfer.RecievedQuantity = 0;

                        var list = new List<DivisionMaterialTransferTrancation>{

                         new() {
                              Quantity = divisionMaterialTransferItem.IssuedQuantity,
                             IssuedQuantity = divisionMaterialTransferItem.IssuedQuantity,
                             RecievedQuantity = 0,
                             DivisionId = divisionMaterialTransferItem.IssuingDivisionId,
                             MaterialId = divisionMaterialTransferItem.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType=VoucherType.Issue

                         },
                            new() {
                              Quantity = divisionMaterialTransferItem.IssuedQuantity,
                             IssuedQuantity = 0,
                             RecievedQuantity = divisionMaterialTransferItem.IssuedQuantity,
                             DivisionId = divisionMaterialTransferItem.ReceivingDivisionId,
                             MaterialId = divisionMaterialTransferItem.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType=VoucherType.Issue

                         }
                         };

                        divisionMaterialTransferTrancations.AddRange(list);


                    }
                    else
                    {

                        divisionMaterialTransfer.RecievedQuantity = divisionMaterialTransferItem.RecievedQuantity;
                        divisionMaterialTransfer.IssuedQuantity = 0;
                        var list = new List<DivisionMaterialTransferTrancation>
                        {

                         new() {
                             Quantity = divisionMaterialTransferItem.RecievedQuantity,
                             IssuedQuantity =0,
                             RecievedQuantity = divisionMaterialTransferItem.RecievedQuantity,
                             DivisionId = divisionMaterialTransferItem.IssuingDivisionId,
                             MaterialId = divisionMaterialTransferItem.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType= VoucherType.Recieve
                         },
                            new() {
                             VoucherType=VoucherType.Recieve,
                             Quantity = divisionMaterialTransferItem.RecievedQuantity,
                             IssuedQuantity = divisionMaterialTransferItem.RecievedQuantity,
                             RecievedQuantity = 0,
                             DivisionId = divisionMaterialTransferItem.ReceivingDivisionId,
                             MaterialId = divisionMaterialTransferItem.MaterialId,
                             CreatedDate = DateTime.UtcNow
                         }
                         };

                        divisionMaterialTransferTrancations.AddRange(list);
                    }
                    // First save the parent record to get its Id
                    divisionMaterialTransfer = await _divisionTransferRepo.Add(divisionMaterialTransfer);

                    // Now set the parent Id on transactions and save them one by one

                    foreach (var transferItem in divisionMaterialTransferTrancations)
                    {
                        transferItem.DivisionMaterialTransferId = divisionMaterialTransfer.Id;
                        await _divisionTransferTrancationRepo.Add(transferItem);
                    }
                    divisionMaterialTransfer.DivisionMaterialTransferTrancations = divisionMaterialTransferTrancations;
                    await UpdateStock(divisionMaterialTransfer);

                    materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                }
            }
            catch (Exception ex)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("MaterialIssueReceiveService:AddMaterialIssueReceive:Method End");
            return materialIssueReceiveResponseDto;
        }

        public async Task UpdateStock(DivisionMaterialTransfer divisionMaterialTransfer)
        {
            try
            {
                int stock = 0, issuedQuantity = 0, recievedQuantity = 0;
                foreach (var transaction in divisionMaterialTransfer.DivisionMaterialTransferTrancations)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        divisionMaterialTransfer.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        divisionMaterialTransfer.Stock += transaction.RecievedQuantity;
                    }
                }

                divisionMaterialTransfer.UpdatedDate = DateTime.UtcNow;
                await _divisionTransferRepo.Update(divisionMaterialTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<DivisionMaterialTransferItemDto> divisionMaterialTransferItemDtos)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto
                = new MaterialIssueReceiveResponseDto();
            foreach (DivisionMaterialTransferItemDto divisionMaterialTransferItemDto in divisionMaterialTransferItemDtos)
            {
                var existingTransaction = await _divisionTransferTrancationRepo.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == divisionMaterialTransferItemDto.MaterialId)
                     .ToListAsync();
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                if (existingTransaction.Any())
                {
                    var issuedCount = existingTransaction.Sum(x => x.IssuedQuantity);
                    var recivedCount = existingTransaction.Sum(x => x.RecievedQuantity);
                    var balancyQuntity = issuedCount - recivedCount;
                    var onboardedMaterial = existingTransaction.Select(x => x.Material).FirstOrDefault();
                    var onboardedQuntity = onboardedMaterial.MaterialQty;
                    if ((balancyQuntity == 0 || balancyQuntity == onboardedQuntity)
                        && divisionMaterialTransferItemDto.VoucherType == VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " for issue";
                        break;
                    }
                    if (divisionMaterialTransferItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }
            return materialIssueReceiveResponseDto;
        }

        public async Task<bool> AddDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            _logger.LogInformation("DivisionMaterialTransferService:AddDivisionMaterialTransferApproval:Method Start");
            
            try
            {
                // Get the DivisionMaterialTransfer by voucher number
                var divisionMaterialTransfer = await _divisionTransferRepo.Query()
                    .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalRequestDto.VoucherNo);
                
                if (divisionMaterialTransfer == null)
                {
                    _logger.LogWarning($"No DivisionMaterialTransfer found with voucher number: {materialTransferApprovalRequestDto.VoucherNo}");
                    return false;
                }
                
                // Check if there's already an approval record for this voucher
                var existingApproval = await _divisionTransferApprovalRepo.Query()
                    .FirstOrDefaultAsync(x => x.DivisionMaterialTransferId == divisionMaterialTransfer.Id);
                
                if (existingApproval != null)
                {
                    _logger.LogInformation($"Approval record already exists for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                    return true; // Already exists, so we consider it a success
                }
                
                // Create a new approval record with Submitted status
                var approval = new DivisionMaterialTransferApproval
                {
                    DivisionMaterialTransferId = divisionMaterialTransfer.Id,
                    IssuerId = materialTransferApprovalRequestDto.CurrentUserId,
                    RecieverId = materialTransferApprovalRequestDto.ReviewerApproverId,
                    ApprovalStatus = ApprovalStatus.Submmitted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                
                await _divisionTransferApprovalRepo.Add(approval);
                var emailSubject = "Material Transfer for Approval";
                var emailBody = "Hi \nMaterials are tramsfered from division with Voucher No:" + materialTransferApprovalRequestDto.VoucherNo +
                    "Please login into system to accept or Reject it.";
                await SendEmail(materialTransferApprovalRequestDto.ReviewerApproverId,
                    emailSubject, emailBody);

                _logger.LogInformation($"Successfully added approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return false;
            }
        }

        public async  Task<bool> UpdateDivisionMaterialTransferStatus(string voucherNo,ApprovalStatus approvalStatus)
        {
            // Get the DivisionMaterialTransfer by voucher number
            var divisionMaterialTransfer = await _divisionTransferRepo.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (divisionMaterialTransfer == null)
            {
                _logger.LogWarning($"No DivisionMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }
            // Check if there's already an approval record for this voucher
            var existingApproval = await _divisionTransferApprovalRepo.Query()
                .FirstOrDefaultAsync(x => x.DivisionMaterialTransferId == divisionMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }
            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate= DateTime.UtcNow;
            await _divisionTransferApprovalRepo.Update(existingApproval);

            var emailSubject = "Material Transfer Status Updated";
            var emailBody = "Hi \nMaterials are tramsfered to division is updated with status:"+ approvalStatus + "\nVoucher No:" + voucherNo +
                "Please login into system to check the status.";
            await SendEmail(existingApproval.IssuerId,
                emailSubject, emailBody);
            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
        }
        public async Task<List<DivisionMaterialTransferApprovalDto>> GetDivisionMaterialTransfersByIssuingDivision(Guid divisionId)
        {
            _logger.LogInformation("DivisionMaterialTransferService:GetDivisionMaterialTransfersByIssuingDivision:Method Start");
            
            try
            {
                // Get all transfers where the specified division is the issuing division
                var transfers = await _divisionTransferRepo.Query()
                    .Where(roleName== "MaterialIssuer" || roleName == "ExecutiveEngineer" ? x => x.IssueDivisionId == divisionId:x=>x.RecieveDivisionId== divisionId)
                    .Include(x => x.IssueDivision)
                    .Include(x => x.RecieveDivision)
                    .Include(x => x.OnBoardedDivision)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();
                
                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for division ID: {divisionId}");
                    return new List<DivisionMaterialTransferApprovalDto>();
                }
                
                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _divisionTransferApprovalRepo.Query()
                    .Where(a => transferIds.Contains(a.DivisionMaterialTransferId))
                    .ToListAsync();
                
                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.DivisionMaterialTransferId, a => a);
                
                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);
                
                var result = new List<DivisionMaterialTransferApprovalDto>();
                
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
                    
                    var dto = new DivisionMaterialTransferApprovalDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingDivision = _mapper.Map<DivisionDto>(firstTransfer.IssueDivision),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingDivision = _mapper.Map<DivisionDto>(firstTransfer.RecieveDivision),
                        OnBoardedDivision = firstTransfer.OnBoardedDivision != null ? _mapper.Map<DivisionDto>(firstTransfer.OnBoardedDivision) : null,
                        Material = _mapper.Map<MaterialDto>(firstTransfer.Material),
                        ApprovalStatus = approvalStatus
                    };
                    if(roleName == "MaterialReciever" && approvalStatus == ApprovalStatus.Submmitted)
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
                
                _logger.LogInformation($"Found {result.Count} transfer vouchers for division ID: {divisionId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving transfers for division ID: {divisionId}");
                throw;
            }
        }

        public async Task<List<DivisionMaterialIssueReceiveItemDto>> GetDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            _logger.LogInformation("DivisionMaterialTransferService:GetDivisionMaterialIssueReceiveByDateRange:Method Start");
            
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<DivisionMaterialIssueReceiveItemDto> divisionMaterialIssueReceiveItemList = new List<DivisionMaterialIssueReceiveItemDto>();
            
            try
            {
                // Get all materials for the specified work package
                var allMaterials = await _material.Query()
                                    .Where(x => x.WorkPackageId == workPackageId)
                                    .Include(x => x.Category)                                   
                                    .ToListAsync();

                // Get transactions within the date range
                var result = await _divisionTransferTrancationRepo.Query()
                           .Where(v => v.CreatedDate >= startOfDay && v.CreatedDate <= endOfDay)
                           .GroupBy(ts => new { ts.MaterialId, ts.DivisionId })
                           .Select(g => new
                           {
                               g.Key.MaterialId,
                               g.Key.DivisionId,
                               ReceivedQuantity = g.Sum(ts => ts.RecievedQuantity),
                               IssuedQuantity = g.Sum(ts => ts.IssuedQuantity)
                           }).ToListAsync();

                // Create a lookup for materials
                var materialLookup = allMaterials.ToDictionary(
                                          m => new { MaterialId = m.Id },
                                          m => m
                );

                // Process transaction results
                foreach (var item in result)
                {
                    int onBoardedQuantity = 0;
                    string divisionName = string.Empty;
                    
                    

                    var key = new { MaterialId = item.MaterialId };
                    var material = materialLookup.GetValueOrDefault(key);
                    // Get the division
                    var division = await _divisionService.GetDivision((Guid)item.DivisionId);

                    if (material != null && material.DivisionId == item.DivisionId)
                    {
                        onBoardedQuantity = material.MaterialQty;
                        divisionName = division.DivisionName;
                    }
                    else
                    {
                        onBoardedQuantity = 0;
                        divisionName = division?.DivisionName ?? "Unknown Division";
                    }

                    var divisionReport = new DivisionMaterialIssueReceiveItemDto()
                    {
                        IssueReceiveDivision = division,
                        Material = _mapper.Map<MaterialDto>(material),
                        IssueReceiveDivisionId = item.DivisionId,
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,
                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity
                    };
                    
                    divisionMaterialIssueReceiveItemList.Add(divisionReport);
                }

                // Add materials that don't have transactions in the date range
                foreach (var item in allMaterials)
                {
                    var division = await _divisionService.GetDivision((Guid)item.DivisionId);

                    if (item.DivisionId != Guid.Empty && item.SubDivisionId==Guid.Empty && item.LocationId==null
                        && !result.Any(r => r.MaterialId == item.Id && r.DivisionId == item.DivisionId.Value))
                    {
                        var divisionReport = new DivisionMaterialIssueReceiveItemDto()
                        {
                            IssueReceiveDivision = division,
                            Material = _mapper.Map<MaterialDto>(item),
                            IssueReceiveDivisionId = item.DivisionId.Value,
                            MaterialId = item.Id,
                            RecievedQuantity = 0,
                            IssuedQuantity = 0,
                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        
                        divisionMaterialIssueReceiveItemList.Add(divisionReport);
                    }
                }

                _logger.LogInformation("DivisionMaterialTransferService:GetDivisionMaterialIssueReceiveByDateRange:Method End");
                return divisionMaterialIssueReceiveItemList.OrderBy(x => x.IssueReceiveDivision.DivisionName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionMaterialIssueReceiveByDateRange");
                throw;
            }
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
