using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs.Account
{
    public class RegisterDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        
        public string Email { get; set; }

       
        public string Password { get; set; }

        
        public string ConfirmPassword { get; set; }
    }
}
