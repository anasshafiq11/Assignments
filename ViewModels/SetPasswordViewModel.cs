using System.ComponentModel.DataAnnotations;

namespace Assignment2.ViewModels
{
    public class SetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
