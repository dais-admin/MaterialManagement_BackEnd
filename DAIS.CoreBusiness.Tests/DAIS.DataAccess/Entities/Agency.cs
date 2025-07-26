namespace DAIS.DataAccess.Entities
{
    public class Agency:BaseEntity
    {
        public string AgencyCode {  get; set; }
        public string AgencyName { get; set;}
        public string AgencyType { get; set;}
        public string? AgencyAddress {  get; set;}
        public string? AgencyEmail { get; set;}
        public string? AgencyPhone { get; set;}
        public string? Remarks {  get; set;}
    }
}
