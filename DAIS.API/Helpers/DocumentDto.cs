namespace DAIS.API.Helpers
{
    public class DocumentDto
    {
        public IFormFile DocumentFile { get; set; }
        public string FileAction { get; set; }
        public Guid MaterialId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
