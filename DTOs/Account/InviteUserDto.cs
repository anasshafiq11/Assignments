namespace UserManagement.DTOs.Account
{
    public class InviteUserDto
    {
        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }
        public string? AdminId { get; set; } 
    }
}
