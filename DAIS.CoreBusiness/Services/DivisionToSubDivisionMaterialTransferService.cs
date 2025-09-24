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
using DAIS.Infrastructure.EmailProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DAIS.CoreBusiness.Services
{
    public class DivisionToSubDivisionMaterialTransferService : IDivisionToSubDivisionMaterialTransferService
    {
        private readonly IGenericRepository<DivisionToSubDivisionMaterialTransfer> _divisionToSubDivisionTransferRepo;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<DivisionToSubDivisionMaterialTransferTrancation> _divisionToSubDivisionTransferTrancationRepo;
        private readonly IGenericRepository<DivisionToSubDivisionMaterialTransferApproval> _divisionToSubDivisionTransferApprovalRepo;
        private readonly ILogger<DivisionToSubDivisionMaterialTransferService> _logger;
        private readonly IDivisionService _divisionService;
        private readonly ISubDivisionService _subDivisionService;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly IUserService _userService;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private string roleName = string.Empty;

        public DivisionToSubDivisionMaterialTransferService(
            IGenericRepository<DivisionToSubDivisionMaterialTransfer> divisionToSubDivisionTransferRepo,
            IGenericRepository<DivisionToSubDivisionMaterialTransferTrancation> divisionToSubDivisionTransferTrancationRepo,
            IGenericRepository<DivisionToSubDivisionMaterialTransferApproval> divisionToSubDivisionTransferApprovalRepo,
            IGenericRepository<Material> material,
            IMapper mapper,
            IDivisionService divisionService,
            ISubDivisionService subDivisionService,
            ILogger<DivisionToSubDivisionMaterialTransferService> logger,
            IUserService userService,
            IEmailService mailService,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _divisionToSubDivisionTransferRepo = divisionToSubDivisionTransferRepo;
            _divisionToSubDivisionTransferTrancationRepo = divisionToSubDivisionTransferTrancationRepo;
            _divisionToSubDivisionTransferApprovalRepo = divisionToSubDivisionTransferApprovalRepo;
            _material = material;
            _divisionService = divisionService;
            _subDivisionService = subDivisionService;
            _mapper = mapper;
            _logger = logger;
            _materialServiceInfrastructure = materialServiceInfrastructure;
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

        public async Task<DivisionToSubDivisionMaterialTransferDto> GetDivisionToSubDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:GetDivisionToSubDivisionMaterialTransferByVoucherNo:Method Start");

            var voucherEntities = _divisionToSubDivisionTransferRepo.Query()
                .Include(x => x.IssueDivision)
                .Include(x => x.TargetSubDivision)
                .Include(x => x.OnBoardedDivision)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var divisionToSubDivisionMaterialTransferDto = new DivisionToSubDivisionMaterialTransferDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                DivisionToSubDivisionMaterialTransferItems = voucherEntities.Select(v => new DivisionToSubDivisionMaterialTransferItemDto
                {
                    IssuingDivisionId = v.IssueDivisionId,
                    IssuingDivision = _mapper.Map<DivisionDto>(v.IssueDivision),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    ReceivingSubDivisionId = v.TargetSubDivisionId,
                    ReceivingSubDivision = _mapper.Map<SubDivisionDto>(v.TargetSubDivision),
                    OnBoardedDivisionId = v.OnBoardedDivisionId,
                    OnBoardedDivision = v.OnBoardedDivision != null ? _mapper.Map<DivisionDto>(v.OnBoardedDivision) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:GetDivisionToSubDivisionMaterialTransferByVoucherNo:Method End");
            return divisionToSubDivisionMaterialTransferDto;
        }

        public async Task<MaterialIssueReceiveResponseDto> AddDivisionToSubDivisionMaterialTransfer(DivisionToSubDivisionMaterialTransferDto divisionToSubDivisionMaterialTransferDto)
        {
            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:AddDivisionToSubDivisionMaterialTransfer:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();

            try
            {
                materialIssueReceiveResponseDto = await CheckMaterialValidForIssueRecive(divisionToSubDivisionMaterialTransferDto.DivisionToSubDivisionMaterialTransferItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }

                foreach (DivisionToSubDivisionMaterialTransferItemDto divisionToSubDivisionMaterialTransferItem in divisionToSubDivisionMaterialTransferDto.DivisionToSubDivisionMaterialTransferItems)
                {
                    var divisionToSubDivisionMaterialTransferTrancations = new List<DivisionToSubDivisionMaterialTransferTrancation>();

                    var checkStock = await _divisionToSubDivisionTransferRepo.GetWhere(x => x.MaterialId == divisionToSubDivisionMaterialTransferItem.MaterialId
                    && (x.IssueDivisionId == divisionToSubDivisionMaterialTransferItem.IssuingDivisionId)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == divisionToSubDivisionMaterialTransferItem.MaterialId
                    && x.DivisionId == divisionToSubDivisionMaterialTransferItem.IssuingDivisionId);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var divisionToSubDivisionMaterialTransfer = new DivisionToSubDivisionMaterialTransfer()
                    {
                        VoucherNo = divisionToSubDivisionMaterialTransferDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(divisionToSubDivisionMaterialTransferDto.Date),
                        VoucherType = divisionToSubDivisionMaterialTransferItem.VoucherType,
                        IssueDivisionId = divisionToSubDivisionMaterialTransferItem.IssuingDivisionId,
                        TargetSubDivisionId = divisionToSubDivisionMaterialTransferItem.ReceivingSubDivisionId,
                        OnBoardedDivisionId = divisionToSubDivisionMaterialTransferItem.OnBoardedDivisionId,
                        IssuedQuantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity,
                        RecievedQuantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = divisionToSubDivisionMaterialTransferItem.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock
                    };

                    if (divisionToSubDivisionMaterialTransferItem.VoucherType == VoucherType.Issue)
                    {
                        divisionToSubDivisionMaterialTransfer.IssuedQuantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity;
                        divisionToSubDivisionMaterialTransfer.RecievedQuantity = 0;

                        var list = new List<DivisionToSubDivisionMaterialTransferTrancation>{
                            new() {
                                Quantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity,
                                RecievedQuantity = 0,
                                DivisionId = divisionToSubDivisionMaterialTransferItem.IssuingDivisionId,
                                SubDivisionId = divisionToSubDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = divisionToSubDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            },
                            new() {
                                Quantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = divisionToSubDivisionMaterialTransferItem.IssuedQuantity,
                                DivisionId = divisionToSubDivisionMaterialTransferItem.IssuingDivisionId,
                                SubDivisionId = divisionToSubDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = divisionToSubDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            }
                        };

                        divisionToSubDivisionMaterialTransferTrancations.AddRange(list);
                    }
                    else
                    {
                        divisionToSubDivisionMaterialTransfer.RecievedQuantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity;
                        divisionToSubDivisionMaterialTransfer.IssuedQuantity = 0;

                        var list = new List<DivisionToSubDivisionMaterialTransferTrancation>{
                            new() {
                                Quantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity,
                                DivisionId = divisionToSubDivisionMaterialTransferItem.IssuingDivisionId,
                                SubDivisionId = divisionToSubDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = divisionToSubDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Recieve
                            },
                            new() {
                                VoucherType = VoucherType.Recieve,
                                Quantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = divisionToSubDivisionMaterialTransferItem.RecievedQuantity,
                                RecievedQuantity = 0,
                                DivisionId = divisionToSubDivisionMaterialTransferItem.IssuingDivisionId,
                                SubDivisionId = divisionToSubDivisionMaterialTransferItem.ReceivingSubDivisionId,
                                MaterialId = divisionToSubDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow
                            }
                        };

                        divisionToSubDivisionMaterialTransferTrancations.AddRange(list);
                    }

                    // First save the parent record to get its Id
                    divisionToSubDivisionMaterialTransfer = await _divisionToSubDivisionTransferRepo.Add(divisionToSubDivisionMaterialTransfer);

                    // Now set the parent Id on transactions and save them one by one
                    foreach (var transferItem in divisionToSubDivisionMaterialTransferTrancations)
                    {
                        transferItem.DivisionToSubDivisionMaterialTransferId = divisionToSubDivisionMaterialTransfer.Id;
                        await _divisionToSubDivisionTransferTrancationRepo.Add(transferItem);
                    }

                    divisionToSubDivisionMaterialTransfer.DivisionToSubDivisionMaterialTransferTrancation = divisionToSubDivisionMaterialTransferTrancations;
                    await UpdateStock(divisionToSubDivisionMaterialTransfer);

                    materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                }
            }
            catch (Exception ex)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:AddDivisionToSubDivisionMaterialTransfer:Method End");
            return materialIssueReceiveResponseDto;
        }

        public async Task UpdateStock(DivisionToSubDivisionMaterialTransfer divisionToSubDivisionMaterialTransfer)
        {
            try
            {
                foreach (var transaction in divisionToSubDivisionMaterialTransfer.DivisionToSubDivisionMaterialTransferTrancation)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        divisionToSubDivisionMaterialTransfer.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        divisionToSubDivisionMaterialTransfer.Stock += transaction.RecievedQuantity;
                    }
                }

                divisionToSubDivisionMaterialTransfer.UpdatedDate = DateTime.UtcNow;
                await _divisionToSubDivisionTransferRepo.Update(divisionToSubDivisionMaterialTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<DivisionToSubDivisionMaterialTransferItemDto> divisionToSubDivisionMaterialTransferItemDtos)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();

            foreach (DivisionToSubDivisionMaterialTransferItemDto divisionToSubDivisionMaterialTransferItemDto in divisionToSubDivisionMaterialTransferItemDtos)
            {
                var existingTransaction = await _divisionToSubDivisionTransferTrancationRepo.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == divisionToSubDivisionMaterialTransferItemDto.MaterialId)
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
                        && divisionToSubDivisionMaterialTransferItemDto.VoucherType == VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " for issue";
                        break;
                    }

                    if (divisionToSubDivisionMaterialTransferItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }

            return materialIssueReceiveResponseDto;
        }

        public async Task<bool> AddDivisionToSubDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:AddDivisionToSubDivisionMaterialTransferApproval:Method Start");

            try
            {
                // Get the DivisionToSubDivisionMaterialTransfer by voucher number
                var divisionToSubDivisionMaterialTransfer = await _divisionToSubDivisionTransferRepo.Query()
                    .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalRequestDto.VoucherNo);

                if (divisionToSubDivisionMaterialTransfer == null)
                {
                    _logger.LogWarning($"No DivisionToSubDivisionMaterialTransfer found with voucher number: {materialTransferApprovalRequestDto.VoucherNo}");
                    return false;
                }

                // Check if there's already an approval record for this voucher
                var existingApproval = await _divisionToSubDivisionTransferApprovalRepo.Query()
                    .FirstOrDefaultAsync(x => x.DivisionToSubDivisionMaterialTransferId == divisionToSubDivisionMaterialTransfer.Id);

                if (existingApproval != null)
                {
                    _logger.LogInformation($"Approval record already exists for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                    return true; // Already exists, so we consider it a success
                }

                // Create a new approval record with Submitted status
                var approval = new DivisionToSubDivisionMaterialTransferApproval
                {
                    DivisionToSubDivisionMaterialTransferId = divisionToSubDivisionMaterialTransfer.Id,
                    IssuerId = materialTransferApprovalRequestDto.CurrentUserId,
                    RecieverId = materialTransferApprovalRequestDto.ReviewerApproverId,
                    ApprovalStatus = ApprovalStatus.Submmitted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _divisionToSubDivisionTransferApprovalRepo.Add(approval);
                var emailSubject = "Material Transfer for Approval";
                var emailBody = "Hi \nMaterials are tramsfered from division to subdivision with Voucher No:" + materialTransferApprovalRequestDto.VoucherNo +
                    "Please login into system to accept or Reject it.";
                //await SendEmail(materialTransferApprovalRequestDto.ReviewerApproverId,
                //    emailSubject, emailBody);

                _logger.LogInformation($"Successfully added approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding approval record for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                return false;
            }
        }

        public async Task<bool> UpdateDivisionToSubDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            // Get the DivisionToSubDivisionMaterialTransfer by voucher number
            var divisionToSubDivisionMaterialTransfer = await _divisionToSubDivisionTransferRepo.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (divisionToSubDivisionMaterialTransfer == null)
            {
                _logger.LogWarning($"No DivisionToSubDivisionMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }

            // Check if there's already an approval record for this voucher
            var existingApproval = await _divisionToSubDivisionTransferApprovalRepo.Query()
                .FirstOrDefaultAsync(x => x.DivisionToSubDivisionMaterialTransferId == divisionToSubDivisionMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }

            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate = DateTime.UtcNow;
            await _divisionToSubDivisionTransferApprovalRepo.Update(existingApproval);

            var emailSubject = "Material Transfer Status Updated";
            var emailBody = "Hi \nMaterials are tramsfered from division to subdivision is updated with status:" + approvalStatus + "\nVoucher No:" + voucherNo +
                "Please login into system to check the status.";
            //await SendEmail(existingApproval.IssuerId,
            //    emailSubject, emailBody);

            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
        }

        public async Task<List<DivisionToSubDivisionMaterialTransferApprovalDto>> GetDivisionToSubDivisionMaterialTransfersByIssuingDivision(Guid divisionId)
        {
            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:GetDivisionToSubDivisionMaterialTransfersByIssuingDivision:Method Start");

            try
            {
                // Get all transfers where the specified division is the issuing division
                var transfers = await _divisionToSubDivisionTransferRepo.Query()
                    .Where(roleName == "MaterialIssuer" || roleName == "ExecutiveEngineer" ? x => x.IssueDivisionId == divisionId : x => x.TargetSubDivisionId == divisionId)
                    .Include(x => x.IssueDivision)
                    .Include(x => x.TargetSubDivision)
                    .Include(x => x.OnBoardedDivision)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();

                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for division ID: {divisionId}");
                    return new List<DivisionToSubDivisionMaterialTransferApprovalDto>();
                }

                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _divisionToSubDivisionTransferApprovalRepo.Query()
                    .Where(a => transferIds.Contains(a.DivisionToSubDivisionMaterialTransferId))
                    .ToListAsync();

                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.DivisionToSubDivisionMaterialTransferId, a => a);

                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);

                var result = new List<DivisionToSubDivisionMaterialTransferApprovalDto>();

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

                    var dto = new DivisionToSubDivisionMaterialTransferApprovalDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingDivision = _mapper.Map<DivisionDto>(firstTransfer.IssueDivision),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingSubDivision = _mapper.Map<SubDivisionDto>(firstTransfer.TargetSubDivision),
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

        public async Task<List<DivisionToSubDivisionMaterialIssueReceiveItemDto>> GetDivisionToSubDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:GetDivisionToSubDivisionMaterialIssueReceiveByDateRange:Method Start");
            
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<DivisionToSubDivisionMaterialIssueReceiveItemDto> divisionToSubDivisionMaterialIssueReceiveItemList = new List<DivisionToSubDivisionMaterialIssueReceiveItemDto>();
            
            try
            {
                // Get all materials for the specified work package
                var allMaterials = await _material.Query()
                                    .Where(x => x.WorkPackageId == workPackageId)
                                    .Include(x => x.Category)                                   
                                    .ToListAsync();

                // Get transactions within the date range
                var result = await _divisionToSubDivisionTransferTrancationRepo.Query()
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
                    string divisionName = string.Empty;
                    
                    var key = new { MaterialId = item.MaterialId };
                    var material = materialLookup.GetValueOrDefault(key);
                    
                    // Get the division and subdivision
                    var division = await _divisionService.GetDivision((Guid)material.DivisionId);
                    var subDivision = await _subDivisionService.GetSubDivision(item.SubDivisionId.Value);

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

                    var divisionToSubDivisionReport = new DivisionToSubDivisionMaterialIssueReceiveItemDto()
                    {
                        IssueReceiveDivisionId = material.DivisionId.Value,
                        IssueReceiveDivision = division,
                        ReceiveSubDivisionId = item.SubDivisionId.Value,
                        ReceiveSubDivision = _mapper.Map<SubDivisionDto>(subDivision),
                        Material = _mapper.Map<MaterialDto>(material),
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,
                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity
                    };
                    
                    divisionToSubDivisionMaterialIssueReceiveItemList.Add(divisionToSubDivisionReport);
                }

                // Add materials that don't have transactions in the date range
                foreach (var item in allMaterials)
                {
                    if (item.DivisionId != Guid.Empty && item.SubDivisionId != null
                        && !result.Any(r => r.MaterialId == item.Id && r.SubDivisionId == item.SubDivisionId.Value))
                    {
                        var division = await _divisionService.GetDivision((Guid)item.DivisionId);
                        var subDivision = await _subDivisionService.GetSubDivision(item.SubDivisionId.Value);

                        var divisionToSubDivisionReport = new DivisionToSubDivisionMaterialIssueReceiveItemDto()
                        {
                            IssueReceiveDivisionId = item.DivisionId.Value,
                            IssueReceiveDivision = division,
                            ReceiveSubDivisionId = item.SubDivisionId.Value,
                            ReceiveSubDivision = _mapper.Map<SubDivisionDto>(subDivision),
                            Material = _mapper.Map<MaterialDto>(item),
                            MaterialId = item.Id,
                            RecievedQuantity = 0,
                            IssuedQuantity = 0,
                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        
                        divisionToSubDivisionMaterialIssueReceiveItemList.Add(divisionToSubDivisionReport);
                    }
                }

                _logger.LogInformation("DivisionToSubDivisionMaterialTransferService:GetDivisionToSubDivisionMaterialIssueReceiveByDateRange:Method End");
                return divisionToSubDivisionMaterialIssueReceiveItemList.OrderBy(x => x.IssueReceiveDivision.DivisionName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDivisionToSubDivisionMaterialIssueReceiveByDateRange");
                throw;
            }
        }
    }
}
