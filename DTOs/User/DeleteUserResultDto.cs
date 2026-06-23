namespace UserManagement.DTOs.User
{
    public class DeleteUserResultDto
    {
        public bool Succeeded { get; set; }

        public bool SelfDeleted { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
