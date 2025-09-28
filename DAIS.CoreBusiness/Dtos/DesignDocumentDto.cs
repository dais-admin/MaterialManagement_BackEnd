using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class DesignDocumentDto
    {
        public Guid ProjectId { get; set; }
        public ProjectDto? project { get; set; }
        public Guid WorkPackageId { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }
        public string DesignDocumentName { get; set; }
        public string DocumentFileName { get; set; }

    }
}
