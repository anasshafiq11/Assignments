namespace UserManagement.Services.Models.User
{
    public class UserList
    {
        public List<User> Users { get; set; } = [];
        public string? Search { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
