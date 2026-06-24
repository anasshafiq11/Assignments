using UserManagement.Services.Models.Account;

namespace Assignment2.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
