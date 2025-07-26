
using DAIS.DataAccess.Helpers;

namespace DAIS.DataAccess.Entities
{
    public class BulkUploadDetail:BaseEntity
    {
        public string FileName {  get; set; }
        public string FilePath { get; set; }
        public int NoOfRecords {  get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public string? Comment {  get; set; }

    }
}
