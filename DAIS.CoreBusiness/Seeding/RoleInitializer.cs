using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAIS.CoreBusiness.Seeding
{
    public static class RoleInitializer
    {
        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            try
            {
                var roles = new[] { "Admin","ExecutiveEngineer",
                    "Approver","Submitter", "Reviewer",
                    "MaterialIssuer","MaterialReciever",
                     "Viewer",
                };
                IdentityResult roleResult;

                foreach (var roleName in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName).ConfigureAwait(false);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new Role { Name = roleName, NormalizedName = roleName }).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
