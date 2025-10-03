using DAIS.CoreBusiness.Dtos.Reports;
using Microsoft.AspNetCore.Http;

namespace DAIS.CoreBusiness.Dtos
{
    public class DesignDocumentDto
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? WorkPackageId { get; set; }
        public WorkPackageDto? WorkPackage { get; set; }
        public string DesignDocumentName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? ResponseMessage { get; set; }
        public bool? IsSuccess { get; set; }
       
        
     
    }
}
