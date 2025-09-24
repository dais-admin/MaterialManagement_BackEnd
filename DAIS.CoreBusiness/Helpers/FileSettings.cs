namespace DAIS.CoreBusiness.Helpers
{
    public class FileSettings
    {
        public string RootDirectory { get; set; }
        public long MaxFileSize { get; set; }   
        public string FileEncryptionKey { get; set; }
        public string FileEncryptionIV { get; set; }
    }
}
