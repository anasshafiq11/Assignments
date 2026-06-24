using System.ComponentModel.DataAnnotations;

namespace UsersApi.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter valid password")]
        public string Password { get; set; } = string.Empty;
    }
}