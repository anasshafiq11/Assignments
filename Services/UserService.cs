using Gridify;
using System.Linq.Expressions;
using UsersApi.Common;
using UsersApi.Common.Querying;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;
using UsersApi.Services.Interfaces;

namespace UsersApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync(QueryOptions queryOptions)
        {
            return await _userRepository.GetPagedAndFilteredAsync(queryOptions);
        }
        public async Task<User?> GetUserAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            return await _userRepository.UpdateAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
    }
}