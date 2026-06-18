using Assignment2.Models;

namespace Assignment2.Services.Interfaces
{
    public interface IUserService
    {
        
        Task<(List<ApplicationUser>, int)> GetUsersAsync(string currentUserId, bool isAdmin, string? search, string? sortOrder, string? filter, int pageNumber, int pageSize);

        Task<ApplicationUser?> GetUserByIdAsync(string id);

        Task CreateUserAsync(ApplicationUser user);

        Task UpdateUserAsync(ApplicationUser user);

        Task DeleteUserAsync(string id);
    }
}
