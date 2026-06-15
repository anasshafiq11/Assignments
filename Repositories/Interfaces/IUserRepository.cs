using UsersApi.Models;

namespace UsersApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<User?> GetUserAsync(int id);
        Task<bool> DeleteUserAsync(int id);
         
    }
}
