using Gridify;
using UsersApi.Common;
using UsersApi.Common.Querying;
using UsersApi.Models;

namespace UsersApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync(QueryOptions queryOptions);
        Task<User?> GetUserAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
