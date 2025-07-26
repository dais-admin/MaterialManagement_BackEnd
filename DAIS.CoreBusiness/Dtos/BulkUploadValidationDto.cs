namespace DAIS.CoreBusiness.Dtos
{
    public class BulkUploadValidationDto
    {
        public string? FieldName {  get; set; }
        public string? FieldValue { get; set; }
        public string? FieldAddress { get; set; }
        public string? ValidationMessage {  get; set; }
        public bool IsValidationSucess {  get; set; }   

    }
}
