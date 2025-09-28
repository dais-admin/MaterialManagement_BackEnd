using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class DivisionLocationMaterialTransferService : IDivisionLocationMaterialTransferService
    {
        private readonly IGenericRepository<DivisionLocationMaterialTransfer> _divisionLocationTransferRepo;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<DivisionLocationMaterialTransferTrancation> _divisionLocationTransferTrancationRepo;
        private readonly IGenericRepository<DivisionLocationMaterialTransferApproval> _divisionLocationTransferApprovalRepo;
        private readonly ILogger<DivisionLocationMaterialTransferService> _logger;
        private readonly IDivisionService _divisionService;
        private readonly ILocationOperationService _locationOperationService;
        private readonly IMapper _mapper;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private string roleName = string.Empty;
        public DivisionLocationMaterialTransferService(
            IGenericRepository<DivisionLocationMaterialTransfer> divisionLocationTransferRepo,
            IGenericRepository<DivisionLocationMaterialTransferTrancation> divisionLocationTransferTrancationRepo,
            IGenericRepository<DivisionLocationMaterialTransferApproval> divisionLocationTransferApprovalRepo,
            IGenericRepository<Material> material, 
            IMapper mapper,
            IDivisionService divisionService,
            ILocationOperationService locationOperationService,
            ILogger<DivisionLocationMaterialTransferService> logger,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _divisionLocationTransferRepo = divisionLocationTransferRepo;
            _divisionLocationTransferTrancationRepo = divisionLocationTransferTrancationRepo;
            _divisionLocationTransferApprovalRepo = divisionLocationTransferApprovalRepo;
            _material = material;
            _divisionService = divisionService;
            _locationOperationService = locationOperationService;
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
        public async Task<DivisionLocationMaterialTransferDto> GetDivisionLocationMaterialTransferByVoucherNo(string voucherNo)
        {
            _logger.LogInformation("DivisionLocationMaterialTransferService:GetDivisionLocationMaterialTransferByVoucherNo:Method Start");
            
            var voucherEntities = _divisionLocationTransferRepo.Query()
                .Include(x => x.IssueDivision)
                .Include(x => x.RecieveLocation)
                .Include(x => x.OnBoardedDivision)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var divisionLocationMaterialTransferDto = new DivisionLocationMaterialTransferDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                DivisionLocationMaterialTransferItems = voucherEntities.Select(v => new DivisionLocationMaterialTransferItemDto
                {
                    IssuingDivisionId = v.IssueDivisionId,
                    IssuingDivision = _mapper.Map<DivisionDto>(v.IssueDivision),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    ReceivingLocationId = v.RecieveLocationId,
                    ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(v.RecieveLocation),
                    OnBoardedDivisionId = v.OnBoardedDivisionId,
                    OnBoardedDivision = v.OnBoardedDivision != null ? _mapper.Map<DivisionDto>(v.OnBoardedDivision) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            _logger.LogInformation("DivisionLocationMaterialTransferService:GetDivisionLocationMaterialTransferByVoucherNo:Method End");
            return divisionLocationMaterialTransferDto;
        }

        public async Task<MaterialIssueReceiveResponseDto> AddDivisionLocationMaterialTransfer(DivisionLocationMaterialTransferDto divisionLocationMaterialTransferDto)
        {
            _logger.LogInformation("DivisionLocationMaterialTransferService:AddDivisionLocationMaterialTransfer:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();
            
            try
            {
                materialIssueReceiveResponseDto = await CheckMaterialValidForIssueRecive(divisionLocationMaterialTransferDto.DivisionLocationMaterialTransferItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }
                
                foreach (DivisionLocationMaterialTransferItemDto divisionLocationMaterialTransferItem in divisionLocationMaterialTransferDto.DivisionLocationMaterialTransferItems)
                {
                    var divisionLocationMaterialTransferTrancations = new List<DivisionLocationMaterialTransferTrancation>();

                    var checkStock = await _divisionLocationTransferRepo.GetWhere(x => x.MaterialId == divisionLocationMaterialTransferItem.MaterialId
                    && (x.IssueDivisionId == divisionLocationMaterialTransferItem.IssuingDivisionId)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == divisionLocationMaterialTransferItem.MaterialId
                    && x.DivisionId == divisionLocationMaterialTransferItem.IssuingDivisionId);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var divisionLocationMaterialTransfer = new DivisionLocationMaterialTransfer()
                    {
                        VoucherNo = divisionLocationMaterialTransferDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(divisionLocationMaterialTransferDto.Date),
                        VoucherType = divisionLocationMaterialTransferItem.VoucherType,
                        IssueDivisionId = divisionLocationMaterialTransferItem.IssuingDivisionId,
                        RecieveLocationId = divisionLocationMaterialTransferItem.ReceivingLocationId,
                        OnBoardedDivisionId = divisionLocationMaterialTransferItem.OnBoardedDivisionId,
                        IssuedQuantity = divisionLocationMaterialTransferItem.IssuedQuantity,
                        RecievedQuantity = divisionLocationMaterialTransferItem.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = divisionLocationMaterialTransferItem.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock
                    };

                    if (divisionLocationMaterialTransferItem.VoucherType == VoucherType.Issue)
                    {
                        divisionLocationMaterialTransfer.IssuedQuantity = divisionLocationMaterialTransferItem.IssuedQuantity;
                        divisionLocationMaterialTransfer.RecievedQuantity = 0;

                        var list = new List<DivisionLocationMaterialTransferTrancation>{
                            new() {
                                Quantity = divisionLocationMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = divisionLocationMaterialTransferItem.IssuedQuantity,
                                RecievedQuantity = 0,
                                LocationId = divisionLocationMaterialTransferItem.ReceivingLocationId,
                                MaterialId = divisionLocationMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            }
                        };

                        divisionLocationMaterialTransferTrancations.AddRange(list);
                    }
                    else
                    {
                        divisionLocationMaterialTransfer.RecievedQuantity = divisionLocationMaterialTransferItem.RecievedQuantity;
                        divisionLocationMaterialTransfer.IssuedQuantity = 0;
                        
                        var list = new List<DivisionLocationMaterialTransferTrancation>{
                            new() {
                                Quantity = divisionLocationMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = divisionLocationMaterialTransferItem.RecievedQuantity,
                                LocationId = divisionLocationMaterialTransferItem.ReceivingLocationId,
                                MaterialId = divisionLocationMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Recieve
                            }
                        };

                        divisionLocationMaterialTransferTrancations.AddRange(list);
                    }
                    
                    // First save the parent record to get its Id
                    divisionLocationMaterialTransfer = await _divisionLocationTransferRepo.Add(divisionLocationMaterialTransfer);

                    // Now set the parent Id on transactions and save them one by one
                    foreach (var transferItem in divisionLocationMaterialTransferTrancations)
                    {
                        transferItem.DivisionMaterialTransferId = divisionLocationMaterialTransfer.Id;
                        await _divisionLocationTransferTrancationRepo.Add(transferItem);
                    }
                    
                    divisionLocationMaterialTransfer.DivisionLocationMaterialTransferTrancations = divisionLocationMaterialTransferTrancations;
                    await UpdateStock(divisionLocationMaterialTransfer);

                    materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                }
            }
            catch (Exception ex)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                _logger.LogError(ex.Message);
            }
            
            _logger.LogInformation("DivisionLocationMaterialTransferService:AddDivisionLocationMaterialTransfer:Method End");
            return materialIssueReceiveResponseDto;
        }

        public async Task UpdateStock(DivisionLocationMaterialTransfer divisionLocationMaterialTransfer)
        {
            try
            {
                foreach (var transaction in divisionLocationMaterialTransfer.DivisionLocationMaterialTransferTrancations)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        divisionLocationMaterialTransfer.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        divisionLocationMaterialTransfer.Stock += transaction.RecievedQuantity;
                    }
                }

                divisionLocationMaterialTransfer.UpdatedDate = DateTime.UtcNow;
                await _divisionLocationTransferRepo.Update(divisionLocationMaterialTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<DivisionLocationMaterialTransferItemDto> divisionLocationMaterialTransferItemDtos)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();
            
            foreach (DivisionLocationMaterialTransferItemDto divisionLocationMaterialTransferItemDto in divisionLocationMaterialTransferItemDtos)
            {
                var existingTransaction = await _divisionLocationTransferTrancationRepo.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == divisionLocationMaterialTransferItemDto.MaterialId)
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
                        && divisionLocationMaterialTransferItemDto.VoucherType == VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " for issue";
                        break;
                    }
                    
                    if (divisionLocationMaterialTransferItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }
            
            return materialIssueReceiveResponseDto;
        }

        public async Task<bool> AddDivisionLocationMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            _logger.LogInformation("DivisionLocationMaterialTransferService:AddDivisionLocationMaterialTransferApproval:Method Start");
            
            try
            {
                // Get the DivisionLocationMaterialTransfer by voucher number
                var divisionLocationMaterialTransfer = await _divisionLocationTransferRepo.Query()
                    .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalRequestDto.VoucherNo);
                
                if (divisionLocationMaterialTransfer == null)
                {
                    _logger.LogWarning($"No DivisionLocationMaterialTransfer found with voucher number: {materialTransferApprovalRequestDto.VoucherNo}");
                    return false;
                }
                
                // Check if there's already an approval record for this voucher
                var existingApproval = await _divisionLocationTransferApprovalRepo.Query()
                    .FirstOrDefaultAsync(x => x.DivisionLocationMaterialTransferId == divisionLocationMaterialTransfer.Id);
                
                if (existingApproval != null)
                {
                    _logger.LogInformation($"Approval record already exists for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                    return true; // Already exists, so we consider it a success
                }
                
                // Create a new approval record with Submitted status
                var approval = new DivisionLocationMaterialTransferApproval
                {
                    DivisionLocationMaterialTransferId = divisionLocationMaterialTransfer.Id,
                    IssuerId = materialTransferApprovalRequestDto.CurrentUserId,
                    RecieverId = materialTransferApprovalRequestDto.ReviewerApproverId,
                    ApprovalStatus = ApprovalStatus.Submmitted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                
                await _divisionLocationTransferApprovalRepo.Add(approval);
                
                _logger.LogInformation($"Successfully added approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return false;
            }
        }
        public async Task<bool> UpdateDivisionLocationMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            // Get the DivisionLocationMaterialTransfer by voucher number
            var divisionLocationMaterialTransfer = await _divisionLocationTransferRepo.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (divisionLocationMaterialTransfer == null)
            {
                _logger.LogWarning($"No divisionLocationMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }
            // Check if there's already an approval record for this voucher
            var existingApproval = await _divisionLocationTransferApprovalRepo.Query()
                .FirstOrDefaultAsync(x => x.DivisionLocationMaterialTransferId == divisionLocationMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }
            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate = DateTime.UtcNow;
            await _divisionLocationTransferApprovalRepo.Update(existingApproval);

            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
        }

        public async Task<List<DivisionLocationMaterialTransferApprovalDto>> GetDivisionLocationMaterialTransfersByIssuingDivision(Guid divisionId)
        {
            _logger.LogInformation("DivisionLocationMaterialTransferService:GetDivisionLocationMaterialTransfersByIssuingDivision:Method Start");
            
            try
            {
                // Get all transfers where the specified division is the issuing division
                var transfers = await _divisionLocationTransferRepo.Query()
                    .Where(roleName == "MaterialIssuer" ? x => x.IssueDivisionId == divisionId : x => x.RecieveLocationId == divisionId)
                    .Include(x => x.IssueDivision)
                    .Include(x => x.RecieveLocation)
                    .Include(x => x.OnBoardedDivision)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();
                
                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for division ID: {divisionId}");
                    return new List<DivisionLocationMaterialTransferApprovalDto>();
                }
                
                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _divisionLocationTransferApprovalRepo.Query()
                    .Where(a => transferIds.Contains(a.DivisionLocationMaterialTransferId))
                    .ToListAsync();
                
                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.DivisionLocationMaterialTransferId, a => a);
                
                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);
                
                var result = new List<DivisionLocationMaterialTransferApprovalDto>();
                
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
                    
                    var dto = new DivisionLocationMaterialTransferApprovalDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingDivision = _mapper.Map<DivisionDto>(firstTransfer.IssueDivision),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingLocation = _mapper.Map<LocationOperationDto>(firstTransfer.RecieveLocation),
                        OnBoardedDivision = firstTransfer.OnBoardedDivision != null ? _mapper.Map<DivisionDto>(firstTransfer.OnBoardedDivision) : null,
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

        public async Task<List<DivisionLocationMaterialIssueReceiveItemDto>> GetDivisionLocationMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            _logger.LogInformation("DivisionLocationMaterialTransferService:GetDivisionLocationMaterialIssueReceiveByDateRange:Method Start");
            
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<DivisionLocationMaterialIssueReceiveItemDto> divisionLocationMaterialIssueReceiveItemList = new List<DivisionLocationMaterialIssueReceiveItemDto>();
            
            try
            {
                // Get all materials for the specified work package
                var allMaterials = await _material.Query()
                                    .Where(x => x.WorkPackageId == workPackageId)
                                    .Include(x => x.Category)                                   
                                    .ToListAsync();

                // Get transactions within the date range
                var result = await _divisionLocationTransferTrancationRepo.Query()
                           .Where(v => v.CreatedDate >= startOfDay && v.CreatedDate <= endOfDay)
                           .GroupBy(ts => new { ts.MaterialId, ts.LocationId })
                           .Select(g => new
                           {
                               g.Key.MaterialId,
                               g.Key.LocationId,
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
                    
                    // Get the division and location
                    var division = await _divisionService.GetDivision((Guid)material.DivisionId);
                    var location = await _locationOperationService.GetLocationOperation(item.LocationId);

                    if (material != null && material.DivisionId != null)
                    {
                        onBoardedQuantity = material.MaterialQty;
                        divisionName = division.DivisionName;
                    }
                    else
                    {
                        onBoardedQuantity = 0;
                        divisionName = division?.DivisionName ?? "Unknown Division";
                    }

                    var divisionLocationReport = new DivisionLocationMaterialIssueReceiveItemDto()
                    {
                        IssuingDivisionId = material.DivisionId.Value,
                        issuingDivision = division,
                        ReceiveLocationId = item.LocationId,
                        ReceiveLocation = _mapper.Map<LocationOperationDto>(location),
                        Material = _mapper.Map<MaterialDto>(material),
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,
                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity
                    };
                    
                    divisionLocationMaterialIssueReceiveItemList.Add(divisionLocationReport);
                }

                // Add materials that don't have transactions in the date range
                foreach (var item in allMaterials)
                {
                    if (item.DivisionId != Guid.Empty && item.LocationId != null
                        && !result.Any(r => r.MaterialId == item.Id && r.LocationId == item.LocationId.Value))
                    {
                        var division = await _divisionService.GetDivision((Guid)item.DivisionId);
                        var location = await _locationOperationService.GetLocationOperation(item.LocationId.Value);

                        var divisionLocationReport = new DivisionLocationMaterialIssueReceiveItemDto()
                        {
                            IssuingDivisionId = item.DivisionId.Value,
                            issuingDivision = division,
                            ReceiveLocationId = item.LocationId.Value,
                            ReceiveLocation = _mapper.Map<LocationOperationDto>(location),
                            Material = _mapper.Map<MaterialDto>(item),
                            MaterialId = item.Id,
                            RecievedQuantity = 0,
                            IssuedQuantity = 0,
                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        
                        divisionLocationMaterialIssueReceiveItemList.Add(divisionLocationReport);
                    }
                }

                _logger.LogInformation("DivisionLocationMaterialTransferService:GetDivisionLocationMaterialIssueReceiveByDateRange:Method End");
                return divisionLocationMaterialIssueReceiveItemList.OrderBy(x => x.issuingDivision.DivisionName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionLocationMaterialIssueReceiveByDateRange");
                throw;
            }
        }
    }
}
