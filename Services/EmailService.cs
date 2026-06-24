using Assignment2.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using UserManagement.Services.Models.Account;

namespace Assignment2.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {

                using var message = new MailMessage();

                message.From = new MailAddress(_emailSettings.Email);
                message.To.Add(emailMessage.ToEmail);
                message.Subject = emailMessage.Subject;
                message.Body = emailMessage.Body;
                message.IsBodyHtml = true;

                using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

                smtpClient.EnableSsl = _emailSettings.EnableSsl;
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Email not sent: {ex.Message}");
            }
        }
}
}