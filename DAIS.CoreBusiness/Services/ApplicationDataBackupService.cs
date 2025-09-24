using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services
{
    public class ApplicationDataBackupService: IApplicationDataBackupService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _backupFolder;
        private readonly string _databaseName;
        private IGenericRepository<AppBackupDetail> _genericRepo;
        private readonly ILogger<ApplicationDataBackupService> _logger;
        public ApplicationDataBackupService(IConfiguration configuration,
            IGenericRepository<AppBackupDetail> genericRepo,
            ILogger<ApplicationDataBackupService> logger)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AppDbConnection");
            _backupFolder = _configuration["BackupSettings:BackupFolder"];
            _databaseName = _configuration["BackupSettings:DatabaseName"];
            _genericRepo = genericRepo;
            _logger = logger;
        }
        public async Task BackupSQLDatabaseAsync()
        {
            var backupDetail = new AppBackupDetailDto();
            try
            {
                var fileName = $"{_databaseName}_{DateTime.Now:yyyyMMddHHmmss}.bak";
                var backupPath = Path.Combine(_backupFolder, fileName);
                var sql = $"BACKUP DATABASE [{_databaseName}] TO DISK = @path WITH INIT";

                backupDetail.BackupType = "Database";
                backupDetail.BackupDateTime = DateTime.Now;
                backupDetail.BackupLocation = backupPath;
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@path", backupPath);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                backupDetail.BackupStatus = "Success";
                backupDetail.BackupSize = 0;
             
                Console.WriteLine($"[✔] Backup completed: {backupPath}");
                await SaveBackupDetails(backupDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✖] Backup failed: {ex.Message}");
                backupDetail.BackupStatus = "Failed";
                await SaveBackupDetails(backupDetail);
            }
        }
        public async Task<bool> SaveBackupDetails(AppBackupDetailDto backupDetail)
        {
            bool isSucess = true;
            try
            {
              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string insertQuery = @"
                    INSERT INTO AppBackupDetails (Id,BackupType,BackupDateTime,BackupLocation, BackupStatus, BackupSize)
                    VALUES (@Id,@BackupType,@BackupDateTime,@BackupLocation, @BackupStatus, @BackupSize)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@BackupType", backupDetail.BackupType);
                        cmd.Parameters.AddWithValue("@BackupDateTime", backupDetail.BackupDateTime);
                        cmd.Parameters.AddWithValue("@BackupLocation", backupDetail.BackupLocation);
                        cmd.Parameters.AddWithValue("@BackupStatus", backupDetail.BackupStatus);
                        cmd.Parameters.AddWithValue("@BackupSize", backupDetail.BackupSize);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving backup.");
            }
            return isSucess;
        }
        public async Task<List<AppBackupDetailDto>> GetLastestBackupDetails()
        {
            List<AppBackupDetailDto> appBackupDetailList=new List<AppBackupDetailDto>();
            try
            {
                var backupDetails=await _genericRepo.Query()
                    .OrderByDescending(x => x.BackupDateTime) // sort by newest first
                    .Take(5) // take the latest 5
                    .ToListAsync();
                foreach (var backupDetail in backupDetails)
                {
                    var backupDto = new AppBackupDetailDto()
                    {
                        BackupStatus = backupDetail.BackupStatus,
                        BackupSize = backupDetail.BackupSize,
                        BackupDateTime = backupDetail.BackupDateTime,
                        BackupLocation = backupDetail.BackupLocation,
                        BackupType = backupDetail.BackupType,
                    };
                    appBackupDetailList.Add(backupDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting backup.");
            }
            return appBackupDetailList;
        }
    }
}
