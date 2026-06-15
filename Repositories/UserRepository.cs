using UserApi.Data;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;

namespace UsersApi.Repositories
{
    public class UserRepository: IUserRepository
    {

        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return null;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            await _context.SaveChangesAsync();
            return existingUser;
        }
        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
