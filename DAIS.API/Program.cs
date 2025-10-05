using Dais.Metrics.Extensions;
using DAIS.API.BackgroundServices;
using DAIS.API.Extensions;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Extensions;
using Dias.ExcelSteam.Connection;
using Dias.ExcelSteam.Extensions;
using DocumentFormat.OpenXml.AdditionalCharacteristics;
using DocumentFormat.OpenXml.Wordprocessing;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.CreateConfiguration();
       
        builder.Host.UseDiasMetrics(_ => { });
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        var connestionString = builder.Configuration.GetConnectionString("AppDbConnection");
        builder.Services.AddDataCollectionServices(connestionString);

        builder.Services.AddCustomeService(builder.Configuration);

        builder.Services.AddCustomCors(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddCustomSwagger();
        builder.Services.AddSignalR();

        //builder.Services.AddExcelSteamServices();

        builder.Services.AddSingleton<ISqlDbConnection>(provider =>
        {
            return new DbConnection(connectionString: connestionString!);
        });

        builder.Services.AddHostedService<CpuUsageMonitoringService>();
        builder.Services.AddHostedService<AppDataBackupService>();
        builder.Services.AddHostedService<AppFileBackupService>();

        var app = builder.Build();

        // Configure Swagger UI in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DAIS API v1");
            });
        }

        app.UseRouting();
        
        // Add HTTPS redirection
        app.UseHttpsRedirection();
        
        app.UseCors("CorsPolicy");
        app.UseCustomMiddleware();
        app.UseCustomStaticFiles(builder.Configuration);

        app.UseMetricsEndpoint();
        app.UseRoleAndUserSeed();

        app.MapControllers();
        
        app.UseEndpoints(endpoints =>
        {
            
            endpoints.MapHub<NotificationHub>("/notificationHub");
        });
        app.Run();
    }  
}
