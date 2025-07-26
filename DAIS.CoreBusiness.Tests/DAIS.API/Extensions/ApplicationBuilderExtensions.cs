using Microsoft.Extensions.FileProviders;
using Serilog;

namespace DAIS.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseErrorHandler();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void UseCustomStaticFiles(this IApplicationBuilder app, IConfiguration configuration)
        {
            var directoryPath = Path.Combine(configuration["MaterialConfig:DocumentBasePath"], "MaterialDocument");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(directoryPath),
                RequestPath = new PathString("/MaterialDocument"),
            });
        }
    }
}
