using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using Dias.ExcelSteam.Conversions;
using Dias.ExcelSteam.Dtos;
using Dias.ExcelSteam.Repositories.CategoryRepo;
using Dias.ExcelSteam.Repositories.DivisionRepo;
using Dias.ExcelSteam.Repositories.LocationOfOperation;
using Dias.ExcelSteam.Repositories.ManufacturersRepo;
using Dias.ExcelSteam.Repositories.MaterialTypeRepo;
using Dias.ExcelSteam.Repositories.ProjectRepo;
using Dias.ExcelSteam.Repositories.RegionRepo;
using Dias.ExcelSteam.Repositories.SupplierRepo;
using Dias.ExcelSteam.Repositories.UnitWork;
using Dias.ExcelSteam.Repositories.WorkPackageRepo;
using Microsoft.Extensions.Logging;

namespace Dias.ExcelSteam.Repositories
{
    public class AddMaterialService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMaterialTypeRepository _materialTypeRepository;
        private readonly ICategoryRepository _category;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IWorkPackageRepository _workPackageRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ILocationOperationRepository _locationOperationRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IMetadataService _metadataService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddMaterialService> _logger;

        public AddMaterialService(
            IProjectRepository projectRepository,
            IMaterialTypeRepository materialTypeRepository,
            IUnitOfWork unitOfWork,
            ICategoryRepository category,
            IDivisionRepository divisionRepository,
            IRegionRepository regionRepository,
            ILocationOperationRepository locationOperationRepository,
            ISupplierRepository supplierRepository,
            IWorkPackageRepository workPackageRepository,
            IManufacturersRepository manufacturersRepository,
            IMetadataService metadataService,
            ILogger<AddMaterialService> logger)
        {
            _projectRepository = projectRepository;
            _materialTypeRepository = materialTypeRepository;
            _category = category;
            _unitOfWork = unitOfWork;
            _divisionRepository = divisionRepository;
            _workPackageRepository = workPackageRepository;
            _regionRepository = regionRepository;
            _locationOperationRepository = locationOperationRepository;
            _supplierRepository = supplierRepository;
            _manufacturersRepository = manufacturersRepository;
            _metadataService = metadataService;
            _logger = logger;

        }

        public async Task CreateMaterialAsync(Guid messageId, MaterialDto materialDto, string? userName = null)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _logger.LogInformation("Starting the UploadMaterialAsync process.");

                    var project = await _projectRepository.GetAsync(materialDto.Project, transaction).ConfigureAwait(false);
                    var workPackage = await _workPackageRepository.GetAsync(materialDto.WorkPackage, transaction).ConfigureAwait(false);
                    var location = await _locationOperationRepository.GetAsync(materialDto.LocationOfOperation, transaction).ConfigureAwait(false);
                    var division = await _divisionRepository.GetAsync(materialDto.Devision, transaction).ConfigureAwait(false);
                    var region = await _regionRepository.GetAsync(materialDto.Region, transaction).ConfigureAwait(false);
                    var materialType = await _materialTypeRepository.GetAsync(materialDto.MaterialType, transaction).ConfigureAwait(false);
                    var category = await _category.GetAsync(materialDto.Category, transaction).ConfigureAwait(false);
                    var supplier = await _supplierRepository.GetAsync(materialDto.Supplier, transaction).ConfigureAwait(false);
                    var manufacturer = await _manufacturersRepository.GetAsync(materialDto.Manufacturer, transaction).ConfigureAwait(false);

                    var material = new Material();                   
                    material.CategoryId = category.Id;
                    material.SupplierId = supplier.Id;
                    material.ManufacturerId = manufacturer.Id;
                    material.SubDivisionId = division.Id;
                    material.LocationId = location.Id;
                    material.RegionId = region.Id;
                    material.Supplier = supplier;
                    material.Manufacturer = manufacturer;
                    material.IsDeleted = false;
                    material.LocationRFId = materialDto.LocationRFId;
                    material.IsRehabilitation = false;
                    material.CreatedBy = userName;
                    material.CommissioningDate = materialDto.CommissioningDate;
                    material.DesignLifeDate = materialDto.DesignLifeDate;
                    material.EndPeriodLifeDate = materialDto.EndPeriodLifeDate;
                    material.MaterialCode = materialDto.MaterialCode;
                    material.MaterialName = materialDto.MaterialName;
                    material.MaterialQty = int.Parse(materialDto.MaterialQty);
                    material.System = materialDto.System;
                    material.ModelNumber = materialDto.ModelNumber;
                    material.TagNumber = materialDto.TagNumber;
                    material.TypeId = materialType.Id;
                    material.WorkPackageId = workPackage.Id;
                    material.YearOfInstallation = materialDto.YearOfInstallation;
                    material.PurchaseDate = materialDto.PurchaseDate.Value;
                    material.MaterialStatus = (MaterialStatus)Enum.Parse(typeof(MaterialStatus), materialDto.MaterialStatus, true);



                    var result = await _metadataService.AddAsync(material, transaction).ConfigureAwait(false);

                    await _metadataService.UpdateExcelReaderMetadataAsync(messageId, transaction).ConfigureAwait(false);

                    await _unitOfWork.CommitAsync();

                    _logger.LogInformation("UploadMaterialAsync successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while Uploading Material.");
                    throw;
                }
            }
        }
    }
}
