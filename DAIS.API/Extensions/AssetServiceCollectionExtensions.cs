using AutoMapper;
using DAIS.API.ExcelReader;
using DAIS.API.Helpers;
using DAIS.CoreBusiness.Extensions;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Interfaces.Report;
using DAIS.CoreBusiness.Services;
using DAIS.CoreBusiness.Services.Reports;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using DAIS.Infrastructure.Cryptography;
using DAIS.Infrastructure.EmailProvider;
using Microsoft.Extensions.DependencyInjection;


namespace DAIS.API.Extensions
{
    public static class AssetServiceCollectionExtensions
    {
        public static IServiceCollection AddAssetCollectionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBusinessCollectionServices();
            services.AddSingleton<IFileEncryptionService, FileEncryptionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IWorkPackageService,WorkPackageService>();
            services.AddScoped<IMaterialTypeService, MaterialTypeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILocationOperationService, LocationOperationService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IDocumentTypeService, DocumnetTypeService>();
            services.AddScoped<IMaterialDocumentService, MaterialDocumentService>();
            services.AddScoped<IMaterialServiceProviderService, MaterialServiceProviderService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMaterialWarrantyService, MaterialWarrantyService>();
            services.AddScoped<IMaterialHardwareService, MaterialHardwareService>();
            services.AddScoped<IMaterialSoftwareService, MaterialSoftwareService>();
            services.AddScoped<IMaterialAuditReportService, MaterialAuditReportService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IMaterialApprovalService, MaterialApprovalService>();
            services.AddScoped<IMaterialBulkUploadService, MaterialBulkUploadService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChartService, ChartDataService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IMaterialMeasurementService, MaterialMeasurementService>();
            services.AddScoped<IContractorService, ContractorService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<ISubDivisionService, SubDivisionService>();
            services.AddScoped<IBulkUploadDetailService, BulkUploadDetailService>();
            services.AddScoped<IMaterialMaintenanceService,MaterialMaintenanceService>();
            services.AddScoped<IMaterialIssueReceiveService, MaterialIssueReceiveService>();            
            services.AddScoped<IMaterialAuditService, MaterialAuditService>();  
            services.AddScoped<IExcelDataImporter, ExcelDataImporter>();
            services.AddScoped<IExcelFileStorageService, ExcelFileStorageService>();


            services.AddScoped<MaterialServiceDependencies>(provider =>
            new MaterialServiceDependencies(
                provider.GetRequiredService<IMaterialTypeService>(),
                provider.GetRequiredService<ICategoryService>(),
                provider.GetRequiredService<ILocationOperationService>(),
                provider.GetRequiredService<IDivisionService>(),
                provider.GetRequiredService<ISubDivisionService>(),
                provider.GetRequiredService<IManufacturerService>(),
                provider.GetRequiredService<IRegionService>(),
                provider.GetRequiredService<ISupplierService>(),
                provider.GetRequiredService<IProjectService>(),
                provider.GetRequiredService<IWorkPackageService>(),               
                provider.GetRequiredService<IMaterialMeasurementService>(),
                provider.GetRequiredService<IContractorService>()
            ));

            services.AddScoped<MaterialServiceInfrastructure>(provider =>
            new MaterialServiceInfrastructure(
                provider.GetRequiredService<IGenericRepository<Material>>(),
                provider.GetRequiredService<IMapper>(),
                provider.GetRequiredService<ILogger<MaterialService>>(),
                provider.GetRequiredService<IHttpContextAccessor>()
            ));

            services.AddScoped<IMaterialService, MaterialService>();

            services.Configure<MaterialConfigSettings>(configuration.GetSection("MaterialConfig"));
            services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
            services.Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));


            return services;
        }
    }
}
