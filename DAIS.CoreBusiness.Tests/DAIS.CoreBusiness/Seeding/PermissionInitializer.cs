using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Seeding
{
    public static class PermissionInitializer
    {
        public static async Task PermissionSeed(AppDbContext context)
        {
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(
                    new Permission { Name = "Create" },
                    new Permission { Name = "Edit" },
                    new Permission { Name = "Delete" },
                    new Permission { Name = "View" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
