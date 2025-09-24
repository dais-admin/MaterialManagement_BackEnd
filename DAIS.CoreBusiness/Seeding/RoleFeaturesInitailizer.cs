using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Seeding
{
    public static class RoleFeaturesInitailizer
    {
        public static async Task SeedRoleFeatures(RoleManager<Role> roleManager, AppDbContext context)
        {

            var role = await roleManager.FindByNameAsync("ADMIN").ConfigureAwait(false);
            var features = await context.Features.ToListAsync().ConfigureAwait(false);
            var permissions = await context.Permissions.ToListAsync().ConfigureAwait(false);
            var roleFeatures = new List<RoleFeature>();

            var existingRoleFeatures = await context.RoleFeatures
                .Where(x => x.RoleId == role.Id).ToListAsync().ConfigureAwait(false);

            // Convert existing role features to a set for quick lookup
            var existingRoleFeatureSet = new HashSet<(Guid FeatureId, Guid PermissionId)>(
                existingRoleFeatures.Select(x => (x.FeatureId, x.PermissionId))
            );


            var newRoleFeatures = new List<RoleFeature>();

            foreach (var feature in features)
            {
                foreach (var permission in permissions)
                {
                    // Check if this combination already exists
                    if (!existingRoleFeatureSet.Contains((feature.Id, permission.Id)))
                    {
                        newRoleFeatures.Add(new RoleFeature
                        {
                            RoleId = role.Id,
                            FeatureId = feature.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            // Add new RoleFeature records to the context
            if (newRoleFeatures.Any())
            {
                await context.RoleFeatures.AddRangeAsync(newRoleFeatures).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            //foreach (var feature in features)
            //{
            //    foreach (var permission in permissions)
            //    {
            //        var temp = context.RoleFeatures.Where(x => x.RoleId == role.Id && x.FeatureId == feature.Id && x.PermissionId == permission.Id);
            //        if (temp.Any())
            //            return;
            //        roleFeatures.Add(new RoleFeature
            //        {
            //            RoleId = role.Id,
            //            FeatureId = feature.Id,
            //            PermissionId = permission.Id
            //        });

            //    }
            //}

            //await context.RoleFeatures.AddRangeAsync(roleFeatures).ConfigureAwait(false);

            //// Save changes in a single call
            //await context.SaveChangesAsync().ConfigureAwait(false);


        }


    }
}
