
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;

namespace DAIS.API.BackgroundServices
{
    public class AppDataBackupService : BackgroundService
    {
        private readonly int _intervalMinutes;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AppDataBackupService(IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _intervalMinutes = int.Parse(_configuration["BackupSettings:IntervalInMinutes"]);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var applicationDataBackupService = scope.ServiceProvider.GetRequiredService<IApplicationDataBackupService>();
                    await applicationDataBackupService.BackupSQLDatabaseAsync();
                    await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);
                }
                    
            }
        }
    }
}
