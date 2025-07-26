using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Seeding
{
    public static class FeatureInitializer
    {
        public static async Task FeatureSeed(AppDbContext context)
        {
            Type featuresType = typeof(ApplicationFeatures);
            PropertyInfo[] properties = featuresType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                var existing = context.Features.FirstOrDefault(x => x.Name == name);
                if (existing is null)
                {
                    context.Features.Add(new Feature
                    {
                        Name = name
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
