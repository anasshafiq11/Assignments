namespace UserManagement.Services.Models.User
{
    public class DeleteUserResult
    {
        public bool Succeeded { get; set; }

        public bool SelfDeleted { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
