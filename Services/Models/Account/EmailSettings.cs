namespace UserManagement.Services.Models.Account
{
    // This class is used to hold the email settings for the application. It is used to configure the email service.
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool EnableSsl { get; set; }
    }
}