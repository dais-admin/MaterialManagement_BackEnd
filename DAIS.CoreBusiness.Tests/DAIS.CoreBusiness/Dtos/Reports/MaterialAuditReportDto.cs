namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class MaterialAuditReportDto
    {
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string System {  get; set; }
        public bool? IsRehabilitation {  get; set; }
        public string CreatedOn {  get; set; }
        public string CreatedBy { get; set;}
        public string UpdatedOn { get;set;}
        public string UpdatedBy { get; set;}
        public bool IsDeleted { get; set; }
        public LocationOperationDto? Location { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }
    }
}
