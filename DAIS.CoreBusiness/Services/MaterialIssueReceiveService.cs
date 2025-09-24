using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DAIS.DataAccess.Helpers;
using Microsoft.Extensions.Logging;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Helpers;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialIssueReceiveService : IMaterialIssueReceiveService
    {
        private readonly IGenericRepository<MaterialIssueRecieveVoucher> _repository;
        private readonly IGenericRepository<Material> _material;
        private readonly IGenericRepository<MaterialVoucherTransaction> _trasaction;
        private readonly ILocationOperationService _locationOperationService;
        private readonly ILogger<MaterialIssueReceiveService> _logger;
        private readonly IMapper _mapper;
        Dictionary<Guid, Dictionary<Guid, int>> locationMaterialStocks = new Dictionary<Guid, Dictionary<Guid, int>>();

        public MaterialIssueReceiveService(
            IGenericRepository<MaterialIssueRecieveVoucher> repository,
            IGenericRepository<Material> material,
           ILocationOperationService locationOperationService,
            IGenericRepository<MaterialVoucherTransaction> trasaction,
            ILogger<MaterialIssueReceiveService> logger, IMapper mapper
           )
        {
            _repository = repository;
            _material = material;
            _logger = logger;
            _mapper = mapper;
            _trasaction = trasaction;
            _locationOperationService = locationOperationService;
        }

        public async Task<MaterialIssueReceiveDto> GetMaterialIssueReceive(Guid id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<MaterialIssueReceiveDto>(entity);
        }

        public async Task<MaterialIssueReceiveDto> GetMaterialIssueReceiveByVoucherNo(string voucherNo)
        {
            var voucherEntities = _repository.Query()
                .Include(x => x.IssueLocation)
                .Include(x => x.RecieveLocation)
                .Include(x => x.OnBoardedLocation)
                .Include(x => x.Material)
                .Where(v => v.VoucherNo == voucherNo).ToList();

            if (!voucherEntities.Any())
                return null;

            var materialIssueReceiveDto = new MaterialIssueReceiveDto
            {
                Id = voucherEntities.First().Id,
                VoucherNo = voucherNo,
                Date = voucherEntities.First().VoucherDate,
                MaterialIssueReceiveItems = voucherEntities.Select(v => new MaterialIssueReceiveItemDto
                {
                    IssuingLocation = v.IssueLocationId,
                    IssuingLocationOperation = _mapper.Map<LocationOperationDto>(v.IssueLocation),
                    VoucherType = v.VoucherType,
                    IssuedQuantity = v.IssuedQuantity,
                    ReceivingLocation = v.RecieveLocationId,
                    ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(v.RecieveLocation),
                    OnBoardedLocationId = v.OnBoardedLocationId,
                    OnBoardedLocationOperation = v.OnBoardedLocation != null ? _mapper.Map<LocationOperationDto>(v.OnBoardedLocation) : null,
                    MaterialId = v.MaterialId,
                    Material = _mapper.Map<MaterialDto>(v.Material)
                }).ToList()
            };

            return materialIssueReceiveDto;
        }

        public async Task<MaterialIssueReceiveResponseDto> AddMaterialIssueReceive(MaterialIssueReceiveDto materialIssueReceiveDto)
        {
            _logger.LogInformation("MaterialIssueReceiveService:AddMaterialIssueReceive:Method Start");
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto =
                new MaterialIssueReceiveResponseDto();
            try
            {

                materialIssueReceiveResponseDto =await CheckMaterialValidForIssueRecive(materialIssueReceiveDto.MaterialIssueReceiveItems);
                if (!materialIssueReceiveResponseDto.IsIssueReceiveSucess)
                {
                    return materialIssueReceiveResponseDto;
                }
                foreach (MaterialIssueReceiveItemDto materialIssueReceiveItemDto in materialIssueReceiveDto.MaterialIssueReceiveItems)
                {
                    var voucherTrancations = new List<MaterialVoucherTransaction>();


                    var checkStock = await _repository.GetWhere(x => x.MaterialId == materialIssueReceiveItemDto.MaterialId
                    && (x.IssueLocationId == materialIssueReceiveItemDto.IssuingLocation)).ConfigureAwait(false);

                    var orderBy = checkStock.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();

                    var checkOnBoardedQuantity = await _material.Query().FirstOrDefaultAsync(x => x.Id == materialIssueReceiveItemDto.MaterialId
                    && x.LocationId == materialIssueReceiveItemDto.IssuingLocation);

                    var onBoardedQuntity = checkOnBoardedQuantity is null ? 0 : checkOnBoardedQuantity.MaterialQty;

                    var materialIssueRecieveVoucher = new MaterialIssueRecieveVoucher()
                    {
                        VoucherNo = materialIssueReceiveDto.VoucherNo,
                        VoucherDate = Convert.ToDateTime(materialIssueReceiveDto.Date),
                        VoucherType = materialIssueReceiveItemDto.VoucherType,
                        IssueLocationId = materialIssueReceiveItemDto.IssuingLocation,
                        RecieveLocationId = materialIssueReceiveItemDto.ReceivingLocation,
                        OnBoardedLocationId = materialIssueReceiveItemDto.OnBoardedLocationId,
                        IssuedQuantity = materialIssueReceiveItemDto.IssuedQuantity,
                        RecievedQuantity = materialIssueReceiveItemDto.RecievedQuantity,
                        OnBoardedQuantity = onBoardedQuntity,
                        MaterialId = materialIssueReceiveItemDto.MaterialId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.UtcNow,
                        Stock = orderBy is null ? onBoardedQuntity : orderBy!.Stock

                    };

                    if (materialIssueReceiveItemDto.VoucherType == VoucherType.Issue)
                    {

                        materialIssueRecieveVoucher.IssuedQuantity = materialIssueReceiveItemDto.IssuedQuantity;
                        materialIssueRecieveVoucher.RecievedQuantity = 0;

                        var list = new List<MaterialVoucherTransaction>{

                         new() {
                              Quantity = materialIssueReceiveItemDto.IssuedQuantity,
                             IssuedQuantity = materialIssueReceiveItemDto.IssuedQuantity,
                             RecievedQuantity = 0,
                             LocationId = materialIssueReceiveItemDto.IssuingLocation,
                             MaterialId = materialIssueReceiveItemDto.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType=VoucherType.Issue

                         },
                            new() {
                              Quantity = materialIssueReceiveItemDto.IssuedQuantity,
                             IssuedQuantity = 0,
                             RecievedQuantity = materialIssueReceiveItemDto.IssuedQuantity,
                             LocationId = materialIssueReceiveItemDto.ReceivingLocation,
                             MaterialId = materialIssueReceiveItemDto.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType=VoucherType.Issue

                         }
                         };

                        voucherTrancations.AddRange(list);


                    }
                    else
                    {

                        materialIssueRecieveVoucher.RecievedQuantity = materialIssueReceiveItemDto.RecievedQuantity;
                        materialIssueRecieveVoucher.IssuedQuantity = 0;
                        var list = new List<MaterialVoucherTransaction>
                        {

                         new() {
                             Quantity = materialIssueReceiveItemDto.RecievedQuantity,
                             IssuedQuantity =0,
                             RecievedQuantity = materialIssueReceiveItemDto.RecievedQuantity,
                             LocationId = materialIssueReceiveItemDto.IssuingLocation,
                             MaterialId = materialIssueReceiveItemDto.MaterialId,
                             CreatedDate = DateTime.UtcNow,
                             VoucherType= VoucherType.Recieve
                         },
                            new() {
                             VoucherType=VoucherType.Recieve,
                             Quantity = materialIssueReceiveItemDto.RecievedQuantity,
                             IssuedQuantity = materialIssueReceiveItemDto.RecievedQuantity,
                             RecievedQuantity = 0,
                             LocationId = materialIssueReceiveItemDto.ReceivingLocation,
                             MaterialId = materialIssueReceiveItemDto.MaterialId,
                             CreatedDate = DateTime.UtcNow
                         }
                         };

                        voucherTrancations.AddRange(list);
                    }
                    // First save the parent record to get its Id
                    materialIssueRecieveVoucher = await _repository.Add(materialIssueRecieveVoucher);

                    // Now set the parent Id on transactions and save them one by one
                    
                    foreach (var transaction in voucherTrancations)
                    {
                        transaction.MaterialIssueRecieveVoucherId = materialIssueRecieveVoucher.Id;
                        await _trasaction.Add(transaction);
                    }
                    materialIssueRecieveVoucher.MaterialVoucherTransactions = voucherTrancations;
                    await UpdateStock(materialIssueRecieveVoucher);

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

        public async Task UpdateStock(MaterialIssueRecieveVoucher voucher)
        {
            try
            {
                int stock = 0, issuedQuantity = 0, recievedQuantity = 0;
                foreach (var transaction in voucher.MaterialVoucherTransactions)
                {
                    if (transaction.VoucherType == VoucherType.Issue)
                    {
                        voucher.Stock -= transaction.IssuedQuantity;
                    }
                    if (transaction.VoucherType == VoucherType.Recieve)
                    {
                        voucher.Stock += transaction.RecievedQuantity;
                    }
                }

                voucher.UpdatedDate = DateTime.UtcNow;
                await _repository.Update(voucher);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private async Task<MaterialIssueReceiveResponseDto> CheckMaterialIssued(Guid materialId)
        {
            var issuedMaterialVoucher = await _repository.Query()
                .Where(x => x.MaterialId == materialId
                && x.VoucherType == VoucherType.Issue
                )
                .Include(x => x.Material)
                .FirstOrDefaultAsync();
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto
                = new MaterialIssueReceiveResponseDto();
            if (issuedMaterialVoucher != null)
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                materialIssueReceiveResponseDto.Message = "Material Code " + issuedMaterialVoucher.Material.MaterialCode
                    + " is already issued";

            }
            else
            {
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
            }
            return materialIssueReceiveResponseDto;
        }

        public async Task<MaterialIssueReceiveDto> UpdateMaterialIssueReceive(MaterialIssueReceiveDto materialIssueReceiveDto)
        {
            var entity = _mapper.Map<MaterialIssueRecieveVoucher>(materialIssueReceiveDto);
            await _repository.Update(entity);
            return materialIssueReceiveDto;
        }

        public async Task DeleteMaterialIssueReceive(Guid id)
        {
            var entity = await _repository.GetById(id);
            if (entity != null)
            {
                await _repository.Remove(entity);
            }
        }

            
        public async Task<MaterialIssueReceiveResponseDto> CheckMaterialValidForIssueRecive(List<MaterialIssueReceiveItemDto> materialIssueReceiveItems)
        {
            MaterialIssueReceiveResponseDto materialIssueReceiveResponseDto
                = new MaterialIssueReceiveResponseDto();
            foreach (MaterialIssueReceiveItemDto materialIssueReceiveItemDto in materialIssueReceiveItems)
            {
                var existingTransaction = await _trasaction.Query()
                     .Include(x => x.Material)
                     .Where(x => x.MaterialId == materialIssueReceiveItemDto.MaterialId)
                     .ToListAsync();
                materialIssueReceiveResponseDto.IsIssueReceiveSucess = true;
                if (existingTransaction.Any())
                {
                    var issuedCount = existingTransaction.Sum(x => x.IssuedQuantity);
                    var recivedCount = existingTransaction.Sum(x => x.RecievedQuantity);
                    var balancyQuntity = issuedCount - recivedCount;
                    var onboardedMaterial = existingTransaction.Select(x=>x.Material).FirstOrDefault();
                    var onboardedQuntity = onboardedMaterial.MaterialQty;
                    if ((balancyQuntity==0 || balancyQuntity == onboardedQuntity) 
                        && materialIssueReceiveItemDto.VoucherType==VoucherType.Issue)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName +" for issue";
                        break;
                    }
                    if(materialIssueReceiveItemDto.VoucherType == VoucherType.Recieve && recivedCount == 0)
                    {
                        materialIssueReceiveResponseDto.IsIssueReceiveSucess = false;
                        materialIssueReceiveResponseDto.Message = "No enough stock available for " + onboardedMaterial.MaterialName + " to recieve";
                        break;
                    }
                }
            }
            return materialIssueReceiveResponseDto;
        }
        public async Task<List<MaterialIssueReceiveDto>> GetAllMaterialIssueReceive()
        {




            var groupedVouchers = _repository.Query()
                .Include(x => x.IssueLocation)
                .Include(x => x.RecieveLocation)
                .Include(x => x.OnBoardedLocation)
                .Include(x => x.Material)
                .GroupBy(v => v.VoucherNo)
                                        .Select(group => new MaterialIssueReceiveDto
                                        {
                                            Id = group.First().Id,
                                            VoucherNo = group.Key,
                                            Date = group.First().VoucherDate,
                                            MaterialIssueReceiveItems = group.Select(v => new MaterialIssueReceiveItemDto
                                            {
                                                IssuingLocation = v.IssueLocationId,
                                                IssuingLocationOperation = _mapper.Map<LocationOperationDto>(v.IssueLocation),
                                                VoucherType = v.VoucherType,
                                                IssuedQuantity = v.IssuedQuantity,
                                                ReceivingLocation = v.RecieveLocationId,
                                                ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(v.RecieveLocation),
                                                OnBoardedLocationId = v.OnBoardedLocationId,
                                                OnBoardedLocationOperation = v.OnBoardedLocation != null ? _mapper.Map<LocationOperationDto>(v.OnBoardedLocation) : null,
                                                MaterialId = v.MaterialId,
                                                Material = _mapper.Map<MaterialDto>(v.Material),


                                            }).ToList()
                                        }).ToList();

            return groupedVouchers;
        }

        public async Task<List<MaterialIssueReceiveDto>> GetMaterialIssueReceiveByDateRange(DateTime fromDate, DateTime toDate)
        {
            var groupedVouchers = _repository.Query()
                .Include(x => x.IssueLocation)
                .Include(x => x.RecieveLocation)
                .Include(x => x.OnBoardedLocation)
                .Include(x => x.Material)
                .Where(v => v.VoucherDate >= fromDate && v.VoucherDate <= toDate)
                .GroupBy(v => v.VoucherNo)
                .Select(group => new MaterialIssueReceiveDto
                {
                    Id = group.First().Id,
                    VoucherNo = group.Key,
                    Date = group.First().VoucherDate,
                    MaterialIssueReceiveItems = group.Select(v => new MaterialIssueReceiveItemDto
                    {
                        IssuingLocation = v.IssueLocationId,
                        IssuingLocationOperation = _mapper.Map<LocationOperationDto>(v.IssueLocation),
                        VoucherType = v.VoucherType,
                        IssuedQuantity = v.IssuedQuantity,
                        RecievedQuantity = v.RecievedQuantity,
                        ReceivingLocation = v.RecieveLocationId,
                        ReceivingLocationOperation = _mapper.Map<LocationOperationDto>(v.RecieveLocation),
                        OnBoardedLocationId = v.OnBoardedLocationId,
                        OnBoardedLocationOperation = v.OnBoardedLocation != null ? _mapper.Map<LocationOperationDto>(v.OnBoardedLocation) : null,
                        MaterialId = v.MaterialId,
                        Material = _mapper.Map<MaterialDto>(v.Material)
                    }).ToList()
                }).ToList();

            return groupedVouchers;
        }
        public async Task<List<MaterialLocatoinIssueReceiveItemDto>> GetMaterialLocationIssueReceiveByDateRange(DateTime fromDate, DateTime toDate,Guid workPackageId)
        {
            var startOfDay = fromDate.Date; // 00:00:00 of the given fromDate
            var endOfDay = toDate.Date.AddDays(1).AddMilliseconds(-1);
            List<MaterialLocatoinIssueReceiveItemDto> materialLocatoinIssueReceiveItemList = new List<MaterialLocatoinIssueReceiveItemDto>(); ;
            try
            {
                
                var allMaterials = await _material.Query()
                                    .Where(x=>x.WorkPackageId==workPackageId)
                                    .Include(x => x.Category)
                                    .Include(x=>x.Location)
                                    .ToListAsync();

                var result = await _trasaction.Query().Where(v => v.CreatedDate >= startOfDay && v.CreatedDate <= endOfDay)
                           .GroupBy(ts => new { ts.MaterialId, ts.LocationId })
                           .Select(g => new
                           {
                               g.Key.MaterialId,
                               g.Key.LocationId,
                               ReceivedQuantity = g.Sum(ts => ts.RecievedQuantity),
                               IssuedQuantity = g.Sum(ts => ts.IssuedQuantity)
                           }).ToListAsync();

                var materialLookup = allMaterials.ToDictionary(
                                          m => new MaterialLocationKey { MaterialId = m.Id },
                                          m => m
);

                foreach (var item in result)
                {
                    int onBoardedQuantity = 0;
                    string LocationName = string.Empty;
                    LocationOperationDto location = await _locationOperationService.GetLocationOperation(item.LocationId).ConfigureAwait(false);
                    var key = new MaterialLocationKey { MaterialId = item.MaterialId };
                    var material = materialLookup.GetValueOrDefault(key);

                    if (material!.LocationId == item.LocationId)
                    {
                        onBoardedQuantity = material!.MaterialQty;
                        LocationName = material.Location.LocationOperationName;
                    }
                    else
                    {
                        onBoardedQuantity = 0;

                        LocationName = location.LocationOperationName;
                    }

                    var locationReport = new MaterialLocatoinIssueReceiveItemDto()
                    {
                        IssueRecieveLocationOperation = location,
                        Material = _mapper.Map<MaterialDto>(material),
                        IssueRecieveLocation = item.LocationId,
                        MaterialId = item.MaterialId,
                        RecievedQuantity = item.ReceivedQuantity,
                        IssuedQuantity = item.IssuedQuantity,

                        BalanceQuantity = onBoardedQuantity + item.ReceivedQuantity - item.IssuedQuantity,
                        OnBoardedQuantity = onBoardedQuantity

                    };
                    materialLocatoinIssueReceiveItemList.Add(locationReport);

                }

                foreach (var item in allMaterials)
                {
                    if (item.LocationId != null && !result.Any(r => r.MaterialId == item.Id && r.LocationId == item.LocationId))
                    {
                        var locationReport = new MaterialLocatoinIssueReceiveItemDto()
                        {
                            IssueRecieveLocationOperation = _mapper.Map<LocationOperationDto>(item.Location),
                            Material = _mapper.Map<MaterialDto>(item),
                            IssueRecieveLocation = item.LocationId!.Value,
                            MaterialId = item.Id,

                            RecievedQuantity = 0,
                            IssuedQuantity = 0,

                            BalanceQuantity = item.MaterialQty,
                            OnBoardedQuantity = item.MaterialQty
                        };
                        materialLocatoinIssueReceiveItemList.Add(locationReport);
                    }
                }

                return materialLocatoinIssueReceiveItemList.OrderBy(x=>x.IssueRecieveLocationOperation.LocationOperationName).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }      
    }
}
