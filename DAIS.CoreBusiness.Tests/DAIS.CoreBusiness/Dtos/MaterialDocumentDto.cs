
using DAIS.CoreBusiness.Dtos.Reports;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialDocumentDto
    {
        public Guid Id { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentFilePath { get; set; }   
        public Guid DocumentTypeId { get; set; }
        public string? DocumentName { get; set; }       
        public Guid MaterialId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ResponseMessage {  get; set; }
        public bool? IsSuccess { get; set; }
        
    }
}
