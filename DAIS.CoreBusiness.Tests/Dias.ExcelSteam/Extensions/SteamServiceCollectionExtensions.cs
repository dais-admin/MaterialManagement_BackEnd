using Dias.ExcelSteam.Connection;
using Dias.ExcelSteam.Conversions;
using Dias.ExcelSteam.DataValidator;
using Dias.ExcelSteam.DataValidator.MaterailValidation;
using Dias.ExcelSteam.Dtos;
using Dias.ExcelSteam.Queue;
using Dias.ExcelSteam.Repositories;
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
using Dias.ExcelSteam.Services;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dias.ExcelSteam.Extensions
{
    public static class SteamServiceCollectionExtensions
    {
        public static IServiceCollection AddExcelSteamServices(this IServiceCollection services)
        {

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<BackgroundWorker>();

            services.AddSingleton<IMetadataService, MetadataService>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton<IDbConnection>(provider =>
            {
                var sqlDbConnection = provider.GetRequiredService<ISqlDbConnection>();
                return new SqlConnection(sqlDbConnection.DbConnetionString);
            });
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            services.AddDataValidator();
            services.AddRepositories();
           // services.AddHostedService<MessageConsumer>();
            services.AddSingleton<IHostedService>(provider => new MessageConsumer(

                provider.GetRequiredService<IMetadataService>(),
                provider.GetRequiredService<IMessageHandler>(),
                provider.GetRequiredService<IDataValidator<MaterialDto>>(),
                provider.GetRequiredService<IBulkInsertMaterial>(),
                provider.GetRequiredService<ILogger<MessageConsumer>>()
                ));
            return services;
        }

        public static IServiceCollection AddDataValidator(this IServiceCollection services)
        {
            services.AddSingleton<IExcelValiationErrorService, ExcelValiationErrorService>();

            services.AddSingleton<IValidator<MaterialDto>, MaterialDtoValidator>();
            services.AddSingleton<IDataValidator<MaterialDto>, MaterialDataValidation>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IMaterialTypeRepository, MaterialTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ILocationOperationRepository, LocationOperationRepository>();
            services.AddScoped<IManufacturersRepository, ManufacturersRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IWorkPackageRepository, WorkPackageRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();
            services.AddScoped<AddMaterialService>();

            services.AddScoped<IBulkInsertMaterial, BulkInsertMaterial>();
            return services;
        }
    }
}
