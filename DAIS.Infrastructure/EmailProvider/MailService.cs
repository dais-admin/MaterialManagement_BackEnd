using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DAIS.Infrastructure.EmailProvider
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(MailData mailData, string[] attachmentPaths = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId));
            message.To.Add(new MailboxAddress(mailData.RecipientName, mailData.RecipientEmail));
            message.Subject = mailData.EmailSubject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = mailData.EmailBody
            };

            // Add attachments if provided
            if (attachmentPaths != null)
            {
                foreach (var attachmentPath in attachmentPaths)
                {
                    if (File.Exists(attachmentPath))
                    {
                        bodyBuilder.Attachments.Add(attachmentPath);
                    }
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await client.AuthenticateAsync(_mailSettings.EmailId, _mailSettings.Password).ConfigureAwait(false);
                    await client.SendAsync(message).ConfigureAwait(false);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error sending email: " + ex.Message);
                    return false;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
                
            }
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                email_Message.To.Add(email_To);
                email_Message.Subject = mailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;
                email_Message.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                SmtpClient MailClient = new SmtpClient();
                MailClient.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
                MailClient.Authenticate(_mailSettings.EmailId, _mailSettings.Password);
                MailClient.Send(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }
    }
}
