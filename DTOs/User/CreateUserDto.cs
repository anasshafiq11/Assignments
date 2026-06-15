using System.ComponentModel.DataAnnotations;

namespace UsersApi.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}