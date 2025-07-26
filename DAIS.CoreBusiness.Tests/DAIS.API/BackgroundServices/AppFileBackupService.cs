using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;

namespace DAIS.API.BackgroundServices
{
    public class AppFileBackupService : BackgroundService
    {
       
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppFileBackupService> _logger;
        private readonly string _sourceFolder =string.Empty;       // Change as needed
        private readonly string _backupFolder = string.Empty;
        private readonly int _intervalInMinutes;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AppFileBackupService(IConfiguration configuration,         
            ILogger<AppFileBackupService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _intervalInMinutes = int.Parse(_configuration["FileBackupSettings:IntervalInMinutes"]);
            _sourceFolder = _configuration["FileBackupSettings:SourceFolder"];
            _backupFolder = _configuration["FileBackupSettings:BackupFolder"];
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string zipPath = Path.Combine(_backupFolder, $"Backup_{timestamp}.zip");
                AppBackupDetailDto appBackupDetailDto = new AppBackupDetailDto();
                try
                {
                    Directory.CreateDirectory(_backupFolder);

                    appBackupDetailDto.BackupStatus = "Sucess";
                    appBackupDetailDto.BackupLocation = zipPath;
                    appBackupDetailDto.BackupType = "File";
                    appBackupDetailDto.BackupDateTime = DateTime.Now;
                    appBackupDetailDto.BackupSize = 0;

                    using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                        {
                            foreach (var filePath in Directory.GetFiles(_sourceFolder, "*", SearchOption.AllDirectories))
                            {
                                if (!IsFileLocked(filePath))
                                {
                                    string entryName = Path.GetRelativePath(_sourceFolder, filePath);
                                    archive.CreateEntryFromFile(filePath, entryName);
                                }
                                else
                                {
                                    _logger.LogWarning("Skipped locked file: {file}", filePath);
                                }
                            }
                        }
                    }
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var applicationDataBackupService = scope.ServiceProvider.GetRequiredService<IApplicationDataBackupService>();
                        await applicationDataBackupService.SaveBackupDetails(appBackupDetailDto);
                    }
                       
                    _logger.LogInformation("Backup completed: {zipPath}", zipPath);
                }
                catch (Exception ex)
                {
                    appBackupDetailDto.BackupStatus = "Failed";
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var applicationDataBackupService = scope.ServiceProvider.GetRequiredService<IApplicationDataBackupService>();
                        await applicationDataBackupService.SaveBackupDetails(appBackupDetailDto);
                    }
                    _logger.LogError(ex, "Error during backup.");
                }

                await Task.Delay(TimeSpan.FromMinutes(_intervalInMinutes), stoppingToken);
            }
        }
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}
