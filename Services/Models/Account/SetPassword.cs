using System.ComponentModel.DataAnnotations;

namespace UserManagement.Services.Models.Account
{
    public class SetPassword
    {
        public string UserId { get; set; }

        public string Token { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
