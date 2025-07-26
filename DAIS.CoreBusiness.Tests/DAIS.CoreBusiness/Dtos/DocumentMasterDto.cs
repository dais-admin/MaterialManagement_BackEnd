namespace DAIS.CoreBusiness.Dtos
{
    public class DocumentMasterDto
    {
        private string documentName;
        public Guid Id { get; set; }
        public string DocumentName
        {
            get => documentName;
            set => documentName = value?.ToUpper();
        }

        public string Remarks {  get; set; }    
    }
}
