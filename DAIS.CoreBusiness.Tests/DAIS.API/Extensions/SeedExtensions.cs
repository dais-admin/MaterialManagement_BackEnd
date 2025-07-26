using DAIS.CoreBusiness.Seeding;
using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAIS.API.Extensions
{
    public static class SeedExtensions
    {
        public static IApplicationBuilder UseRoleAndUserSeed(this IApplicationBuilder app)
        {
            SeedData(app).GetAwaiter().GetResult();
            return app;

        }

        private static async Task SeedData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Seed roles
            await RoleInitializer.SeedRoles(roleManager).ConfigureAwait(false);

            // Seed users
            await UserInitializer.SeedUsers(userManager).ConfigureAwait(false);
            
            await FeatureInitializer.FeatureSeed(context).ConfigureAwait(false);

            await PermissionInitializer.PermissionSeed(context).ConfigureAwait(false);

            await RoleFeaturesInitailizer.SeedRoleFeatures(roleManager, context).ConfigureAwait(false);
        }
    }
}
