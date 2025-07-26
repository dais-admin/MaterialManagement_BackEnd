using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace DAIS.DataAccess.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataCollectionServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

            }).AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;             
                options.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            return services;
        }
    }
}
