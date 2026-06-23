using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;

namespace UsersApi.Repositories
{
    public class UserRepository: GenericRepository<User> ,IUserRepository
    {

        public UserRepository(AppDbContext context): base(context) { }
        
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
}
}
