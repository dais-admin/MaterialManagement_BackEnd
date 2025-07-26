namespace DAIS.Infrastructure.EmailProvider
{
    public class MailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string RecipientName { get; set; }//EmailToId and RecipientEmail are same
        public string RecipientEmail { get; set; }//EmailToName and RecipientEmail are same
    }
}
