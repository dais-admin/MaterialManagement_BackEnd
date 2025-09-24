﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using AutoMapper;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.MaterialTransferDto;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class SubDivisionMaterialTransferService : ISubDivisionMaterialTransferService
    {
        private readonly IGenericRepository<SubDivisionMaterialTransfer> _subDivisionTransferRepo;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<SubDivisionMaterialTransferTransaction> _subDivisionTransferTransactionRepo;
        private readonly IGenericRepository<SubDivisionMaterialTransferApproval> _subDivisionTransferApprovalRepo;
        private readonly ILogger<SubDivisionMaterialTransferService> _logger;
        private readonly ISubDivisionService _subDivisionService;
        private readonly IMapper _mapper;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private string roleName = string.Empty;
        public SubDivisionMaterialTransferService(
            IGenericRepository<SubDivisionMaterialTransfer> subDivisionTransferRepo,
            IGenericRepository<SubDivisionMaterialTransferTransaction> subDivisionTransferTransactionRepo,
            IGenericRepository<SubDivisionMaterialTransferApproval> subDivisionTransferApprovalRepo,
            IGenericRepository<Material> material,
            ISubDivisionService subDivisionService,
            IMapper mapper,
            ILogger<SubDivisionMaterialTransferService> logger,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _subDivisionTransferRepo = subDivisionTransferRepo;
            _subDivisionTransferTransactionRepo = subDivisionTransferTransactionRepo;
            _subDivisionTransferApprovalRepo = subDivisionTransferApprovalRepo;
            _material = material;
            _subDivisionService= subDivisionService;
            _mapper = mapper;
            _logger = logger;
            _materialServiceInfrastructure = materialServiceInfrastructure;
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
        public async Task<SubDivisionMaterialTransferDto> GetSubDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialTransferByVoucherNo:Method Start");
            
            var voucherEntities = _subDivisionTransferRepo.Query()
                .Include(x => x.IssueSubDivision)
                .Include(x => x.RecieveSubDivision)
                .Include(x => x.OnBoardedSubDivision)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var subDivisionMaterialTransferDto = new SubDivisionMaterialTransferDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                SubDivisionMaterialTransferItems = voucherEntities.Select(v => new SubDivisionMaterialTransferItemDto
                {
                    IssuingSubDivisionId = v.IssueSubDivisionId,
                    IssuingSubDivision = _mapper.Map<SubDivisionDto>(v.IssueSubDivision),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    ReceivingSubDivisionId = v.RecieveSubDivisionId,
                    ReceivingSubDivision = _mapper.Map<SubDivisionDto>(v.RecieveSubDivision),
                    OnBoardedSubDivisionId = v.OnBoardedSubDivisionId,
                    OnBoardedSubDivision = v.OnBoardedSubDivision != null ? _mapper.Map<SubDivisionDto>(v.OnBoardedSubDivision) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialTransferByVoucherNo:Method End");
            return subDivisionMaterialTransferDto;
        }

        public async Task<MaterialIssueReceiveResponseDto> AddSubDivisionMaterialTransfer(SubDivisionMaterialTransferDto subDivisionMaterialTransferDto)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:AddSubDivisionMaterialTransfer:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();
            
            try
            {
                materialIssueReceiveResponseDto = await CheckMaterialValidForIssueRecive(subDivisionMaterialTransferDto.SubDivisionMaterialTransferItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }
                foreach (SubDivisionMaterialTransferItemDto subDivisionMaterialTransferItem in subDivisionMaterialTransferDto.SubDivisionMaterialTransferItems)
                {
                    var subDivisionMaterialTransferTransactions = new List<SubDivisionMaterialTransferTransaction>();

                    var checkStock = await _subDivisionTransferRepo.GetWhere(x => x.MaterialId == subDivisionMaterialTransferItem.MaterialId
                    && (x.IssueSubDivisionId == subDivisionMaterialTransferItem.IssuingSubDivisionId)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == subDivisionMaterialTransferItem.MaterialId
                    && x.SubDivisionId == subDivisionMaterialTransferItem.IssuingSubDivisionId);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var subDivisionMaterialTransfer = new SubDivisionMaterialTransfer()
                    {
                        VoucherNo = subDivisionMaterialTransferDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(subDivisionMaterialTransferDto.Date),
                        VoucherType = subDivisionMaterialTransferItem.VoucherType,
                        IssueSubDivisionId = subDivisionMaterialTransferItem.IssuingSubDivisionId,
                        RecieveSubDivisionId = subDivisionMaterialTransferItem.ReceivingSubDivisionId,
                        OnBoardedSubDivisionId = subDivisionMaterialTransferItem.OnBoardedSubDivisionId,
                        IssuedQuantity = subDivisionMaterialTransferItem.IssuedQuantity,
                        RecievedQuantity = subDivisionMaterialTransferItem.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = subDivisionMaterialTransferItem.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock
                    };

                    if (subDivisionMaterialTransferItem.VoucherType == VoucherType.Issue)
                    {
                        subDivisionMaterialTransfer.IssuedQuantity = subDivisionMaterialTransferItem.IssuedQuantity;
                        subDivisionMaterialTransfer.RecievedQuantity = 0;

                        var list = new List<SubDivisionMaterialTransferTransaction>{
                            new() {
                                Quantity = subDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = subDivisionMaterialTransferItem.IssuedQuantity,
                                RecievedQuantity = 0,
                                SubDivisionId = subDivisionMaterialTransferItem.IssuingSubDivisionId,
                                MaterialId = subDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            },
                            new() {
                                Quantity = subDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = subDivisionMaterialTransferItem.IssuedQuantity,
                                SubDivisionId = subDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = subDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            }
                        };

                        subDivisionMaterialTransferTransactions.AddRange(list);
                    }
                    else
                    {
                        subDivisionMaterialTransfer.RecievedQuantity = subDivisionMaterialTransferItem.RecievedQuantity;
                        subDivisionMaterialTransfer.IssuedQuantity = 0;
                        
                        var list = new List<SubDivisionMaterialTransferTransaction>
                        {
                            new() {
                                Quantity = subDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = subDivisionMaterialTransferItem.RecievedQuantity,
                                SubDivisionId = subDivisionMaterialTransferItem.IssuingSubDivisionId,
                                MaterialId = subDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Recieve
                            },
                            new() {
                                VoucherType = VoucherType.Recieve,
                                Quantity = subDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = subDivisionMaterialTransferItem.RecievedQuantity,
                                RecievedQuantity = 0,
                                SubDivisionId = subDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = subDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow
                            }
                        };

                        subDivisionMaterialTransferTransactions.AddRange(list);
                    }
                    
                    // First save the parent record to get its Id
                    subDivisionMaterialTransfer = await _subDivisionTransferRepo.Add(subDivisionMaterialTransfer);

                    // Now set the parent Id on transactions and save them one by one
                    foreach (var transferItem in subDivisionMaterialTransferTransactions)
                    {
                        transferItem.SubDivisionMaterialTransferId = subDivisionMaterialTransfer.Id;
                        await _subDivisionTransferTransactionRepo.Add(transferItem);
                    }
                    
                    subDivisionMaterialTransfer.SubDivisionMaterialTransferTrancations = subDivisionMaterialTransferTransactions;
                    await UpdateStock(subDivisionMaterialTransfer);

                    materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                }
            }
            catch (Exception ex)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                _logger.LogError(ex.Message);
            }
            
            _logger.LogInformation("SubDivisionMaterialTransferService:AddSubDivisionMaterialTransfer:Method End");
            return materialIssueReceiveResponseDto;
        }

        public async Task UpdateStock(SubDivisionMaterialTransfer subDivisionMaterialTransfer)
        {
            try
            {
                foreach (var transaction in subDivisionMaterialTransfer.SubDivisionMaterialTransferTrancations)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        subDivisionMaterialTransfer.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        subDivisionMaterialTransfer.Stock += transaction.RecievedQuantity;
                    }
                }

                subDivisionMaterialTransfer.UpdatedDate = DateTime.UtcNow;
                await _subDivisionTransferRepo.Update(subDivisionMaterialTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<SubDivisionMaterialTransferItemDto> subDivisionMaterialTransferItemDtos)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto
                = new MaterialIssueReceiveResponseDto();
            foreach (SubDivisionMaterialTransferItemDto subDivisionMaterialTransferItemDto in subDivisionMaterialTransferItemDtos)
            {
                var existingTransaction = await _subDivisionTransferTransactionRepo.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == subDivisionMaterialTransferItemDto.MaterialId)
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
                        && subDivisionMaterialTransferItemDto.VoucherType == VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " for issue";
                        break;
                    }
                    if (subDivisionMaterialTransferItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }
            return materialIssueReceiveResponseDto;
        }

        public async Task<bool> AddSubDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:AddSubDivisionMaterialTransferApproval:Method Start");
            
            try
            {
                // Get the SubDivisionMaterialTransfer by voucher number
                var subDivisionMaterialTransfer = await _subDivisionTransferRepo.Query()
                    .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalRequestDto.VoucherNo);
                
                if (subDivisionMaterialTransfer == null)
                {
                    _logger.LogWarning($"No SubDivisionMaterialTransfer found with voucher number: {materialTransferApprovalRequestDto.VoucherNo}");
                    return false;
                }
                
                // Check if there's already an approval record for this voucher
                var existingApproval = await _subDivisionTransferApprovalRepo.Query()
                    .FirstOrDefaultAsync(x => x.SubDivisionMaterialTransferId == subDivisionMaterialTransfer.Id);
                
                if (existingApproval != null)
                {
                    _logger.LogInformation($"Approval record already exists for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                    return true; // Already exists, so we consider it a success
                }
                
                // Create a new approval record with Submitted status
                var approval = new SubDivisionMaterialTransferApproval
                {
                    SubDivisionMaterialTransferId = subDivisionMaterialTransfer.Id,
                    IssuerId = materialTransferApprovalRequestDto.CurrentUserId,
                    RecieverId = materialTransferApprovalRequestDto.ReviewerApproverId,
                    ApprovalStatus = ApprovalStatus.Submmitted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                
                await _subDivisionTransferApprovalRepo.Add(approval);
                
                _logger.LogInformation($"Successfully added approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return false;
            }
        }
        public async Task<bool> UpdateSubDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            // Get the SuDivisionMaterialTransfer by voucher number
            var subDivisionMaterialTransfer = await _subDivisionTransferRepo.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (subDivisionMaterialTransfer == null)
            {
                _logger.LogWarning($"No DivisionMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }
            // Check if there's already an approval record for this voucher
            var existingApproval = await _subDivisionTransferApprovalRepo.Query()
                .FirstOrDefaultAsync(x => x.SubDivisionMaterialTransferId == subDivisionMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }
            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate = DateTime.UtcNow;
            await _subDivisionTransferApprovalRepo.Update(existingApproval);

            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
        }

        public async Task<List<SubDivisionMaterialTransferApprovalDto>> GetSubDivisionMaterialTransfersByIssuingSubDivision(Guid subDivisionId)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialTransfersByIssuingSubDivision:Method Start");
            
            try
            {
                // Get all transfers where the specified subdivision is the issuing subdivision
                var transfers = await _subDivisionTransferRepo.Query()
                    .Where(roleName == "MaterialIssuer" || roleName == "ExecutiveEngineer" ? x => x.IssueSubDivisionId == subDivisionId : x => x.RecieveSubDivisionId == subDivisionId)
                    .Include(x => x.IssueSubDivision)
                    .Include(x => x.RecieveSubDivision)
                    .Include(x => x.OnBoardedSubDivision)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();
                
                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for subdivision ID: {subDivisionId}");
                    return new List<SubDivisionMaterialTransferApprovalDto>();
                }
                
                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _subDivisionTransferApprovalRepo.Query()
                    .Where(a => transferIds.Contains(a.SubDivisionMaterialTransferId))
                    .ToListAsync();
                
                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.SubDivisionMaterialTransferId, a => a);
                
                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);
                
                var result = new List<SubDivisionMaterialTransferApprovalDto>();
                
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
                    
                    var dto = new SubDivisionMaterialTransferApprovalDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingSubDivision = _mapper.Map<SubDivisionDto>(firstTransfer.IssueSubDivision),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingSubDivision = _mapper.Map<SubDivisionDto>(firstTransfer.RecieveSubDivision),
                        OnBoardedSubDivision = firstTransfer.OnBoardedSubDivision != null ? _mapper.Map<SubDivisionDto>(firstTransfer.OnBoardedSubDivision) : null,
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
                
                _logger.LogInformation($"Found {result.Count} transfer vouchers for subdivision ID: {subDivisionId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving transfers for subdivision ID: {subDivisionId}");
                throw;
            }
        }

        public async Task<List<SubDivisionMaterialIssueReceiveItemDto>> GetSubDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialIssueReceiveByDateRange:Method Start");
            
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<SubDivisionMaterialIssueReceiveItemDto> subDivisionMaterialIssueReceiveItemList = new List<SubDivisionMaterialIssueReceiveItemDto>();
            
            try
            {
                // Get all materials for the specified work package
                var allMaterials = await _material.Query()
                                    .Where(x => x.WorkPackageId == workPackageId)
                                    .Include(x => x.Category)                                   
                                    .ToListAsync();

                // Get transactions within the date range
                var result = await _subDivisionTransferTransactionRepo.Query()
                           .Where(v => v.CreatedDate >= startOfDay && v.CreatedDate <= endOfDay)
                           .GroupBy(ts => new { ts.MaterialId, ts.SubDivisionId })
                           .Select(g => new
                           {
                               g.Key.MaterialId,
                               g.Key.SubDivisionId,
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
                    string subDivisionName = string.Empty;

                    // Get the subdivision
                    var subDivision = await _subDivisionService.GetSubDivision(item.SubDivisionId);
                                        
                    
                    var key = new { MaterialId = item.MaterialId };
                    var material = materialLookup.GetValueOrDefault(key);

                    if (material != null && material.SubDivisionId == item.SubDivisionId)
                    {
                        onBoardedQuantity = material.MaterialQty;
                        subDivisionName = subDivision.SubDivisionName ?? "Unknown SubDivision";
                    }
                    else
                    {
                        onBoardedQuantity = 0;
                        subDivisionName = subDivision?.SubDivisionName ?? "Unknown SubDivision";
                    }

                    var subDivisionReport = new SubDivisionMaterialIssueReceiveItemDto()
                    {
                        IssueReceiveSubDivision = subDivision,
                        Material = _mapper.Map<MaterialDto>(material),
                        IssueReceiveSubDivisionId = item.SubDivisionId,
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,
                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity
                    };
                    
                    subDivisionMaterialIssueReceiveItemList.Add(subDivisionReport);
                }

                // Add materials that don't have transactions in the date range
                foreach (var item in allMaterials)
                {
                    var subDivision = await _subDivisionService.GetSubDivision((Guid)item.SubDivisionId);

                    if (item.SubDivisionId != Guid.Empty && item.LocationId==null
                        && !result.Any(r => r.MaterialId == item.Id && r.SubDivisionId == item.SubDivisionId.Value))
                    {
                        var subDivisionReport = new SubDivisionMaterialIssueReceiveItemDto()
                        {
                            IssueReceiveSubDivision = subDivision,
                            Material = _mapper.Map<MaterialDto>(item),
                            IssueReceiveSubDivisionId = item.SubDivisionId.Value,
                            MaterialId = item.Id,
                            RecievedQuantity = 0,
                            IssuedQuantity = 0,
                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        
                        subDivisionMaterialIssueReceiveItemList.Add(subDivisionReport);
                    }
                }

                _logger.LogInformation("SubDivisionMaterialTransferService:GetSubDivisionMaterialIssueReceiveByDateRange:Method End");
                return subDivisionMaterialIssueReceiveItemList.OrderBy(x => x.IssueReceiveSubDivision.SubDivisionName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSubDivisionMaterialIssueReceiveByDateRange");
                throw;
            }
        }
        
    }
}
