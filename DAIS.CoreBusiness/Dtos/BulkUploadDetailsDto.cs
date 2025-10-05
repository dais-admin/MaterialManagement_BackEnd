using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos
{
    public class BulkUploadDetailsDto
    {
        public Guid? Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int NoOfRecords { get; set; }
        public string ChangedBy {  get; set; }
        public DateTime ChangedDate { get; set;}
        public string? ApprovalStatus { get; set; }
        public string? Comment { get; set; }
    }
}
