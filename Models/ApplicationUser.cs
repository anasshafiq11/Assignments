using Microsoft.AspNetCore.Identity;

namespace Assignment2.Models
{
    // The ApplicationUser class extends the IdentityUser class provided by ASP.NET Core Identity, which includes properties for user authentication and authorization such as UserName, Email, PasswordHash, etc. 
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? CreatedByAdminId { get; set; }
    }
}
