using DAIS.CoreBusiness.Interfaces;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialServiceDependencies
    {

        public IMaterialTypeService MaterialTypeService { get; }
        public ICategoryService CategoryService { get; }
        public ILocationOperationService LocationOperationService { get; }
        public IDivisionService DivisionService { get; }
        public ISubDivisionService SubDivisionService { get; }
        public IManufacturerService ManufacturerService { get; }
        public IRegionService RegionService { get; }
        public ISupplierService SupplierService { get; }
        public IProjectService ProjectService { get; }
        public IWorkPackageService WorkPackageService { get; }
        public IMaterialMeasurementService MaterialMeasurementService { get; }
        public IContractorService ContractorService { get; }

        public MaterialServiceDependencies(IMaterialTypeService materialTypeService,
            ICategoryService categoryService,
            ILocationOperationService locationOperationService,
            IDivisionService divisionService,
            ISubDivisionService subDivisionService,
            IManufacturerService manufacturerService,
            IRegionService regionService,
            ISupplierService supplierService,
            IProjectService projectService,
            IWorkPackageService workPackageService,
            IMaterialMeasurementService materialMeasurementService,
            IContractorService contractorService)
        {
            MaterialTypeService = materialTypeService;
            CategoryService = categoryService;
            LocationOperationService = locationOperationService;
            DivisionService = divisionService;
            SubDivisionService = subDivisionService;
            ManufacturerService = manufacturerService;
            RegionService = regionService;
            SupplierService = supplierService;
            ProjectService = projectService;
            WorkPackageService = workPackageService;
            MaterialMeasurementService = materialMeasurementService;
            ContractorService = contractorService;
        }
    }
}
