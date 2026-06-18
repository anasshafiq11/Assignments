using Assignment2.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Assignment2.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            //Console.WriteLine($"Email: {_emailSettings.Email}");
            //Console.WriteLine($"Password Length: {_emailSettings.Password?.Length}");
            //Console.WriteLine($"Host: {_emailSettings.Host}");
            //Console.WriteLine($"Port: {_emailSettings.Port}");
            //Console.WriteLine($"SSL: {_emailSettings.EnableSsl}");

            using var message = new MailMessage();

            message.From = new MailAddress(_emailSettings.Email);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port);

            smtpClient.UseDefaultCredentials = false; // tells .NET not to use your local computer's Windows login data.

            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

            smtpClient.EnableSsl = _emailSettings.EnableSsl;

            await smtpClient.SendMailAsync(message);
        }
    }
}