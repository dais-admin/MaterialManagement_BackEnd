using System.ComponentModel.DataAnnotations;

namespace DAIS.DataAccess.Entities
{
    public class AppBackupDetail
    {
        [Key]
        public Guid Id { get; set; }
        public string BackupType {  get; set; }
        public DateTime BackupDateTime { get; set; }
        public string BackupStatus { get; set; }
        public string BackupLocation {  get; set; }
        public int BackupSize { get; set; }
    }
}
