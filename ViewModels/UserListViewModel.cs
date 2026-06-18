using Assignment2.Models;

namespace Assignment2.ViewModels
{
    public class UserListViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public string? Search { get; set; }
        public string? SortOrder { get; set; }
        public string? Filter { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
