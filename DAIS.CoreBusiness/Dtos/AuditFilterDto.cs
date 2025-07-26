namespace DAIS.CoreBusiness.Dtos
{
    public class AuditFilterDto
    {
        public string TableName {  get; set; }
        public string? UserId {  get; set; }
        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate {  get; set; }
    }
}
