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
    public class SubDivisionToDivisionMaterialTransferService : ISubDivisionToDivisionMaterialTransferService
    {
        private readonly IGenericRepository<SubDivisionToDivisionMaterialTransfer> _subDivisionToDivisionTransferRepo;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<SubDivisionToDivisionMaterialTransferTrancation> _subDivisionToDivisionTransferTrancationRepo;
        private readonly IGenericRepository<SubDivisionToDivisionMaterialTransferApproval> _subDivisionToDivisionTransferApprovalRepo;
        private readonly ILogger<SubDivisionToDivisionMaterialTransferService> _logger;
        private readonly IDivisionService _divisionService;
        private readonly ISubDivisionService _subDivisionService;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly IUserService _userService;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;

        private string userName = string.Empty;
        private string roleName = string.Empty;

        public SubDivisionToDivisionMaterialTransferService(
            IGenericRepository<SubDivisionToDivisionMaterialTransfer> subDivisionToDivisionTransferRepo,
            IGenericRepository<SubDivisionToDivisionMaterialTransferTrancation> subDivisionToDivisionTransferTrancationRepo,
            IGenericRepository<SubDivisionToDivisionMaterialTransferApproval> subDivisionToDivisionTransferApprovalRepo,
            IGenericRepository<Material> material,
            IMapper mapper,
            IDivisionService divisionService,
            ISubDivisionService subDivisionService,
            ILogger<SubDivisionToDivisionMaterialTransferService> logger,
            IUserService userService,
            IEmailService mailService,
            MaterialServiceInfrastructure materialServiceInfrastructure)
        {
            _subDivisionToDivisionTransferRepo = subDivisionToDivisionTransferRepo;
            _subDivisionToDivisionTransferTrancationRepo = subDivisionToDivisionTransferTrancationRepo;
            _subDivisionToDivisionTransferApprovalRepo = subDivisionToDivisionTransferApprovalRepo;
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

        public async Task<SubDivisionToDivisionMaterialTransferDto> GetSubDivisionToDivisionMaterialTransferByVoucherNo(string voucherNo)
        {
            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:GetSubDivisionToDivisionMaterialTransferByVoucherNo:Method Start");

            var voucherEntities = _subDivisionToDivisionTransferRepo.Query()
                .Include(x => x.IssueSubDivision)
                .Include(x => x.RecieveDivision)
                .Include(x => x.OnBoardedSubDivision)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var subDivisionToDivisionMaterialTransferDto = new SubDivisionToDivisionMaterialTransferDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                SubDivisionToDivisionMaterialTransferItems = voucherEntities.Select(v => new SubDivisionToDivisionMaterialTransferItemDto
                {
                    IssueSubDivisionId = v.IssueSubDivisionId,
                    IssueSubDivision = _mapper.Map<SubDivisionDto>(v.IssueSubDivision),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    RecievedQuantity = v.RecievedQuantity,
                    RecieveDivisionId = v.RecieveDivisionId,
                    RecieveDivision = _mapper.Map<DivisionDto>(v.RecieveDivision),
                    OnBoardedSubDivisionId = v.OnBoardedSubDivisionId,
                    OnBoardedSubDivision = v.OnBoardedSubDivision != null ? _mapper.Map<SubDivisionDto>(v.OnBoardedSubDivision) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:GetSubDivisionToDivisionMaterialTransferByVoucherNo:Method End");
            return subDivisionToDivisionMaterialTransferDto;
        }

        public async Task<MaterialIssueReceiveResponseDto> AddSubDivisionToDivisionMaterialTransfer(SubDivisionToDivisionMaterialTransferDto subDivisionToDivisionMaterialTransferDto)
        {
            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:AddSubDivisionToDivisionMaterialTransfer:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();

            try
            {
                materialIssueReceiveResponseDto = await CheckMaterialValidForIssueRecive(subDivisionToDivisionMaterialTransferDto.SubDivisionToDivisionMaterialTransferItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }

                foreach (SubDivisionToDivisionMaterialTransferItemDto subDivisionToDivisionMaterialTransferItem in subDivisionToDivisionMaterialTransferDto.SubDivisionToDivisionMaterialTransferItems)
                {
                    var subDivisionToDivisionMaterialTransferTrancations = new List<SubDivisionToDivisionMaterialTransferTrancation>();

                    var checkStock = await _subDivisionToDivisionTransferRepo.GetWhere(x => x.MaterialId == subDivisionToDivisionMaterialTransferItem.MaterialId
                    && (x.IssueSubDivisionId == subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == subDivisionToDivisionMaterialTransferItem.MaterialId
                    && x.SubDivisionId == subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var subDivisionToDivisionMaterialTransfer = new SubDivisionToDivisionMaterialTransfer()
                    {
                        VoucherNo = subDivisionToDivisionMaterialTransferDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(subDivisionToDivisionMaterialTransferDto.Date),
                        VoucherType = subDivisionToDivisionMaterialTransferItem.VoucherType,
                        IssueSubDivisionId = subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId,
                        RecieveDivisionId = subDivisionToDivisionMaterialTransferItem.RecieveDivisionId,
                        OnBoardedSubDivisionId = subDivisionToDivisionMaterialTransferItem.OnBoardedSubDivisionId,
                        IssuedQuantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity,
                        RecievedQuantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = subDivisionToDivisionMaterialTransferItem.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock
                    };

                    if (subDivisionToDivisionMaterialTransferItem.VoucherType == VoucherType.Issue)
                    {
                        subDivisionToDivisionMaterialTransfer.IssuedQuantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity;
                        subDivisionToDivisionMaterialTransfer.RecievedQuantity = 0;

                        var list = new List<SubDivisionToDivisionMaterialTransferTrancation>{
                            new() {
                                Quantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity,
                                RecievedQuantity = 0,
                                SubDivisionId = subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId,
                                DivisionId = subDivisionToDivisionMaterialTransferItem.RecieveDivisionId,
                                MaterialId = subDivisionToDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            },
                            new() {
                                Quantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = subDivisionToDivisionMaterialTransferItem.IssuedQuantity,
                                SubDivisionId = subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId,
                                DivisionId = subDivisionToDivisionMaterialTransferItem.RecieveDivisionId,
                                MaterialId = subDivisionToDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Issue
                            }
                        };

                        subDivisionToDivisionMaterialTransferTrancations.AddRange(list);
                    }
                    else
                    {
                        subDivisionToDivisionMaterialTransfer.RecievedQuantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity;
                        subDivisionToDivisionMaterialTransfer.IssuedQuantity = 0;

                        var list = new List<SubDivisionToDivisionMaterialTransferTrancation>{
                            new() {
                                Quantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = 0,
                                RecievedQuantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity,
                                SubDivisionId = subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId,
                                DivisionId = subDivisionToDivisionMaterialTransferItem.RecieveDivisionId,
                                MaterialId = subDivisionToDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow,
                                VoucherType = VoucherType.Recieve
                            },
                            new() {
                                VoucherType = VoucherType.Recieve,
                                Quantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity,
                                IssuedQuantity = subDivisionToDivisionMaterialTransferItem.RecievedQuantity,
                                RecievedQuantity = 0,
                                SubDivisionId = subDivisionToDivisionMaterialTransferItem.IssueSubDivisionId,
                                DivisionId = subDivisionToDivisionMaterialTransferItem.RecieveDivisionId,
                                MaterialId = subDivisionToDivisionMaterialTransferItem.MaterialId,
                                CreatedDate = DateTime.UtcNow
                            }
                        };

                        subDivisionToDivisionMaterialTransferTrancations.AddRange(list);
                    }

                    // First save the parent record to get its Id
                    subDivisionToDivisionMaterialTransfer = await _subDivisionToDivisionTransferRepo.Add(subDivisionToDivisionMaterialTransfer);

                    // Now set the parent Id on transactions and save them one by one
                    foreach (var transferItem in subDivisionToDivisionMaterialTransferTrancations)
                    {
                        transferItem.SubDivToDivMaterialTransferId = subDivisionToDivisionMaterialTransfer.Id;
                        await _subDivisionToDivisionTransferTrancationRepo.Add(transferItem);
                    }

                    subDivisionToDivisionMaterialTransfer.SubDivisionToDivisionMaterialTransferTrancations = subDivisionToDivisionMaterialTransferTrancations;
                    await UpdateStock(subDivisionToDivisionMaterialTransfer);

                    materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                }
            }
            catch (Exception ex)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:AddSubDivisionToDivisionMaterialTransfer:Method End");
            return materialIssueReceiveResponseDto;
        }

        public async Task UpdateStock(SubDivisionToDivisionMaterialTransfer subDivisionToDivisionMaterialTransfer)
        {
            try
            {
                foreach (var transaction in subDivisionToDivisionMaterialTransfer.SubDivisionToDivisionMaterialTransferTrancations)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        subDivisionToDivisionMaterialTransfer.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        subDivisionToDivisionMaterialTransfer.Stock += transaction.RecievedQuantity;
                    }
                }

                subDivisionToDivisionMaterialTransfer.UpdatedDate = DateTime.UtcNow;
                await _subDivisionToDivisionTransferRepo.Update(subDivisionToDivisionMaterialTransfer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<SubDivisionToDivisionMaterialTransferItemDto> subDivisionToDivisionMaterialTransferItemDtos)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto = new MaterialIssueReceiveResponseDto();

            foreach (SubDivisionToDivisionMaterialTransferItemDto subDivisionToDivisionMaterialTransferItemDto in subDivisionToDivisionMaterialTransferItemDtos)
            {
                var existingTransaction = await _subDivisionToDivisionTransferTrancationRepo.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == subDivisionToDivisionMaterialTransferItemDto.MaterialId)
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
                        && subDivisionToDivisionMaterialTransferItemDto.VoucherType == VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " for issue";
                        break;
                    }

                    if (subDivisionToDivisionMaterialTransferItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }

            return materialIssueReceiveResponseDto;
        }

        public async Task<bool> AddSubDivisionToDivisionMaterialTransferApproval(MaterialTransferApprovalRequestDto materialTransferApprovalRequestDto)
        {
            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:AddSubDivisionToDivisionMaterialTransferApproval:Method Start");

            try
            {
                // Get the SubDivisionToDivisionMaterialTransfer by voucher number
                var subDivisionToDivisionMaterialTransfer = await _subDivisionToDivisionTransferRepo.Query()
                    .FirstOrDefaultAsync(x => x.VoucherNo == materialTransferApprovalRequestDto.VoucherNo);

                if (subDivisionToDivisionMaterialTransfer == null)
                {
                    _logger.LogWarning($"No SubDivisionToDivisionMaterialTransfer found with voucher number: {materialTransferApprovalRequestDto.VoucherNo}");
                    return false;
                }

                // Check if there's already an approval record for this voucher
                var existingApproval = await _subDivisionToDivisionTransferApprovalRepo.Query()
                    .FirstOrDefaultAsync(x => x.SubDivToDivMaterialTransferApprovalId == subDivisionToDivisionMaterialTransfer.Id);

                if (existingApproval != null)
                {
                    _logger.LogInformation($"Approval record already exists for voucher: {materialTransferApprovalRequestDto.VoucherNo}");
                    return true; // Already exists, so we consider it a success
                }

                // Create a new approval record with Submitted status
                var approval = new SubDivisionToDivisionMaterialTransferApproval
                {
                    SubDivToDivMaterialTransferApprovalId = subDivisionToDivisionMaterialTransfer.Id,
                    IssuerId = materialTransferApprovalRequestDto.CurrentUserId,
                    RecieverId = materialTransferApprovalRequestDto.ReviewerApproverId,
                    ApprovalStatus = ApprovalStatus.Submmitted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _subDivisionToDivisionTransferApprovalRepo.Add(approval);
                var emailSubject = "Material Transfer for Approval";
                var emailBody = "Hi \nMaterials are tramsfered from subdivision to division with Voucher No:" + materialTransferApprovalRequestDto.VoucherNo +
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

        public async Task<bool> UpdateSubDivisionToDivisionMaterialTransferStatus(string voucherNo, ApprovalStatus approvalStatus)
        {
            // Get the SubDivisionToDivisionMaterialTransfer by voucher number
            var subDivisionToDivisionMaterialTransfer = await _subDivisionToDivisionTransferRepo.Query()
                .FirstOrDefaultAsync(x => x.VoucherNo == voucherNo);

            if (subDivisionToDivisionMaterialTransfer == null)
            {
                _logger.LogWarning($"No SubDivisionToDivisionMaterialTransfer found with voucher number: {voucherNo}");
                return false;
            }

            // Check if there's already an approval record for this voucher
            var existingApproval = await _subDivisionToDivisionTransferApprovalRepo.Query()
                .FirstOrDefaultAsync(x => x.SubDivToDivMaterialTransferApprovalId == subDivisionToDivisionMaterialTransfer.Id);

            if (existingApproval.ApprovalStatus == approvalStatus)
            {
                _logger.LogInformation($"Approval record already exists for voucher: {voucherNo}");
                return true; // Already exists, so we consider it a success
            }

            existingApproval.ApprovalStatus = approvalStatus;
            existingApproval.UpdatedDate = DateTime.UtcNow;
            await _subDivisionToDivisionTransferApprovalRepo.Update(existingApproval);

            var emailSubject = "Material Transfer Status Updated";
            var emailBody = "Hi \nMaterials are tramsfered from subdivision to division is updated with status:" + approvalStatus + "\nVoucher No:" + voucherNo +
                "Please login into system to check the status.";
            //await SendEmail(existingApproval.IssuerId,
            //    emailSubject, emailBody);

            _logger.LogInformation($"Successfully added approval record for voucher: {voucherNo}");
            return true;
        }

        public async Task<List<SubDivisionToDivisionMaterialTransferApprovalDto>> GetSubDivisionToDivisionMaterialTransfersByIssuingSubDivision(Guid subDivisionId)
        {
            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:GetSubDivisionToDivisionMaterialTransfersByIssuingSubDivision:Method Start");

            try
            {
                // Get all transfers where the specified subdivision is the issuing subdivision
                var transfers = await _subDivisionToDivisionTransferRepo.Query()
                    .Where(roleName == "MaterialIssuer" || roleName == "ExecutiveEngineer" ? x => x.IssueSubDivisionId == subDivisionId : x => x.RecieveDivisionId == subDivisionId)
                    .Include(x => x.IssueSubDivision)
                    .Include(x => x.RecieveDivision)
                    .Include(x => x.OnBoardedSubDivision)
                    .Include(x => x.Material)
                    .OrderByDescending(x => x.VoucherDate)
                    .ToListAsync();

                if (!transfers.Any())
                {
                    _logger.LogInformation($"No transfers found for subdivision ID: {subDivisionId}");
                    return new List<SubDivisionToDivisionMaterialTransferApprovalDto>();
                }

                // Get all approval records for these transfers
                var transferIds = transfers.Select(t => t.Id).ToList();
                var approvals = await _subDivisionToDivisionTransferApprovalRepo.Query()
                    .Where(a => transferIds.Contains(a.SubDivToDivMaterialTransferApprovalId))
                    .ToListAsync();

                // Create a lookup for approvals by transfer ID
                var approvalLookup = approvals.ToDictionary(a => a.SubDivToDivMaterialTransferApprovalId, a => a);

                // Group transfers by voucher number
                var groupedTransfers = transfers.GroupBy(x => x.VoucherNo);

                var result = new List<SubDivisionToDivisionMaterialTransferApprovalDto>();

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

                    var dto = new SubDivisionToDivisionMaterialTransferApprovalDto
                    {
                        VoucherNo = voucherNo,
                        Date = firstTransfer.VoucherDate,
                        IssuingSubDivision = _mapper.Map<SubDivisionDto>(firstTransfer.IssueSubDivision),
                        VoucherType = firstTransfer.VoucherType,
                        IssuedQuantity = firstTransfer.IssuedQuantity,
                        RecievedQuantity = firstTransfer.RecievedQuantity,
                        OnBoardedQuantity = firstTransfer.OnBoardedQuantity,
                        ReceivingDivision = _mapper.Map<DivisionDto>(firstTransfer.RecieveDivision),
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

        public async Task<List<SubDivisionToDivisionMaterialIssueReceiveItemDto>> GetSubDivisionToDivisionMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate, Guid workPackageId)
        {
            _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:GetSubDivisionToDivisionMaterialIssueReceiveByDateRange:Method Start");
            
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<SubDivisionToDivisionMaterialIssueReceiveItemDto> subDivisionToDivisionMaterialIssueReceiveItemList = new List<SubDivisionToDivisionMaterialIssueReceiveItemDto>();
            
            try
            {
                // Get all materials for the specified work package
                var allMaterials = await _material.Query()
                                    .Where(x => x.WorkPackageId == workPackageId)
                                    .Include(x => x.Category)                                   
                                    .ToListAsync();

                // Get transactions within the date range
                var result = await _subDivisionToDivisionTransferTrancationRepo.Query()
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
                    string subDivisionName = string.Empty;
                    
                    var key = new { MaterialId = item.MaterialId };
                    var material = materialLookup.GetValueOrDefault(key);
                    
                    // Get the subdivision and division
                    var division = await _divisionService.GetDivision(item.DivisionId.Value);
                    var subDivision = await _subDivisionService.GetSubDivision((Guid)material.SubDivisionId);

                    if (material != null && material.SubDivisionId != null)
                    {
                        onBoardedQuantity = material.MaterialQty;
                        subDivisionName = subDivision.SubDivisionName;
                    }
                    else
                    {
                        onBoardedQuantity = 0;
                        subDivisionName = subDivision?.SubDivisionName ?? "Unknown SubDivision";
                    }

                    var subDivisionToDivisionReport = new SubDivisionToDivisionMaterialIssueReceiveItemDto()
                    {
                        IssueReceiveSubDivisionId = material.SubDivisionId.Value,
                        IssueReceiveSubDivision = _mapper.Map<SubDivisionDto>(subDivision),
                        ReceiveDivisionId = item.DivisionId.Value,
                        ReceiveDivision = division,
                        Material = _mapper.Map<MaterialDto>(material),
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,
                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity
                    };
                    
                    subDivisionToDivisionMaterialIssueReceiveItemList.Add(subDivisionToDivisionReport);
                }

                // Add materials that don't have transactions in the date range
                foreach (var item in allMaterials)
                {
                    if (item.SubDivisionId != null && item.DivisionId != Guid.Empty
                        && !result.Any(r => r.MaterialId == item.Id && r.DivisionId == item.DivisionId))
                    {
                        var division = await _divisionService.GetDivision((Guid)item.DivisionId);
                        var subDivision = await _subDivisionService.GetSubDivision(item.SubDivisionId.Value);

                        var subDivisionToDivisionReport = new SubDivisionToDivisionMaterialIssueReceiveItemDto()
                        {
                            IssueReceiveSubDivisionId = item.SubDivisionId.Value,
                            IssueReceiveSubDivision = _mapper.Map<SubDivisionDto>(subDivision),
                            ReceiveDivisionId = item.DivisionId.Value,
                            ReceiveDivision = division,
                            Material = _mapper.Map<MaterialDto>(item),
                            MaterialId = item.Id,
                            RecievedQuantity = 0,
                            IssuedQuantity = 0,
                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        
                        subDivisionToDivisionMaterialIssueReceiveItemList.Add(subDivisionToDivisionReport);
                    }
                }

                _logger.LogInformation("SubDivisionToDivisionMaterialTransferService:GetSubDivisionToDivisionMaterialIssueReceiveByDateRange:Method End");
                return subDivisionToDivisionMaterialIssueReceiveItemList.OrderBy(x => x.IssueReceiveSubDivision.SubDivisionName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSubDivisionToDivisionMaterialIssueReceiveByDateRange");
                throw;
            }
        }
    }
}
