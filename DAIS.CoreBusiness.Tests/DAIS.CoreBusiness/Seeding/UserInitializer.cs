using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAIS.CoreBusiness.Seeding
{
    public static class UserInitializer
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@test.com").ConfigureAwait(false);
            if (user is null)
            {
                user = new User
                {
                    Email = "admin@test.com",
                    EmailConfirmed = true,
                    UserName = "admin",
                    
                };
                await userManager.CreateAsync(user, "Admin@12345");

                await userManager.AddToRoleAsync(user, "Admin").ConfigureAwait(false);
            }
        }
    }
}
