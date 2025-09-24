namespace DAIS.CoreBusiness.Dtos
{
    public class AppBackupDetailDto
    {
        public string BackupType { get; set; }
        public DateTime BackupDateTime { get; set; }
        public string BackupStatus { get; set; }
        public string BackupLocation { get; set; }
        public int BackupSize { get; set; }
    }
}
