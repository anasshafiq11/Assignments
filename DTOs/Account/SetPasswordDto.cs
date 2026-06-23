using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs.Account
{
    public class SetPasswordDto
    {
        public string UserId { get; set; }

        public string Token { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
