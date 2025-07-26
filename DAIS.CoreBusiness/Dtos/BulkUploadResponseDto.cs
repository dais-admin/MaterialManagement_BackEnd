
namespace DAIS.CoreBusiness.Dtos
{
    public class BulkUploadResponseDto
    {
        public List<BulkUploadValidationDto> Validations { get; set; }
        public List<MaterialDto> Materials { get; set; }
        public string UploadedFile { get; set; }
        public string ResponseMessage {  get; set; }
        public BulkUploadDetailsDto BulkUploadDetails {  get; set; }
        public bool IsSuccess { get; set; }
    }
}
