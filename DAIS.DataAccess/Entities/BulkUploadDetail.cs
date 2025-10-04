namespace DAIS.DataAccess.Entities
{
    public class BulkUploadDetail:BaseEntity
    {
        public string FileName {  get; set; }
        public string FilePath { get; set; }
        public int NoOfRecords {  get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ActionRequiredByUserEmail {  get; set; }
        public string? Comment {  get; set; }

    }
}
