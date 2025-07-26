namespace DAIS.Infrastructure.EmailProvider
{
    public interface IEmailService
    {
        bool SendMail(MailData mailData);
        Task<bool> SendEmailAsync(MailData mailData, string[] attachmentPaths = null);
    }
}
