using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IApplicationDataBackupService
    {
        Task BackupSQLDatabaseAsync();
        Task<bool> SaveBackupDetails(AppBackupDetailDto backupDetail);
        Task<List<AppBackupDetailDto>> GetLastestBackupDetails();
    }
}
