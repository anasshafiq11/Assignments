namespace UserManagement.DTOs.User
{
    public class UserListDto
    {
        public List<UserDto> Users { get; set; } = [];
        public string? Search { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
