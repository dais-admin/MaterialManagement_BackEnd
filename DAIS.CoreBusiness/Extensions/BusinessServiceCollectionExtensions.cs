using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using DAIS.DataAccess.Repositories;
using DAIS.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DAIS.CoreBusiness.Extensions
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessCollectionServices(this IServiceCollection services)
        {
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBulkUploadRepository<>), typeof(BulkUploadRepository<>));
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            
            return services;
        }
    }
}
