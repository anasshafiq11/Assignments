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
        private readonly IGenericRepository<User> _repository;

        public UserService(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> GetAllUsersAsync(QueryOptions queryOptions)
        {
            return await _repository.GetPagedAndFilteredAsync(queryOptions);
        }
        public async Task<User?> GetUserAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            return await _repository.UpdateAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repository.GetAsync(u => u.Email == email);
        }
    }
}