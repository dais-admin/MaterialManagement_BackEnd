

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class WorkPackageDto
    {
        private string workPackageName;
        public Guid? Id { get; set; }
        public string WorkPackageName
        {
            get => workPackageName;
            set => workPackageName = value?.ToUpper();
        }
        public string WorkPackageCode { get; set; }
        public string? System { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LetterOfAcceptanceDate { get; set; }
        public DateTime? CommencementDate { get; set; }
        public string? ContractPackageDetails { get; set; }
        public string Remarks {  get; set; }
        public Guid? ProjectId { get; set; }
        public ProjectDto? Project { get; set; }
    }
}
