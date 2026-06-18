using Assignment2.Models;
using Assignment2.Repositories.Interfaces;
using Assignment2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<ApplicationUser> _genericRepository;

        public UserService(IGenericRepository<ApplicationUser> genericRepository)
        {
            _genericRepository = genericRepository;
        }


        public async Task<(List<ApplicationUser>, int)> GetUsersAsync(string currentUserId, bool isAdmin, string? search,string? sortOrder, string? filter,int pageNumber,int pageSize)
        {
            var query = _genericRepository.Query();

            if (isAdmin)
            {
                // Admin sees only users he created
                query = query.Where(x => x.CreatedByAdminId == currentUserId);
            } 
            else
            {
                // User sees users created by same admin
                var currentUser = await _genericRepository.Query().FirstOrDefaultAsync(x => x.Id == currentUserId);

                if (currentUser != null)
                {
                    query = query.Where(x => x.CreatedByAdminId == currentUser.CreatedByAdminId);
                }
            }
            //query = query.Where(x => x.PasswordHash != null); // Exclude users without a password
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FirstName!.Contains(search) ||
                    x.LastName!.Contains(search) ||
                    x.Email!.Contains(search));
            }
            if (!string.IsNullOrEmpty(filter))
            {
                query = filter switch
                {
                    "A" => query.Where(x => x.FirstName!.StartsWith("A")),
                    "B" => query.Where(x => x.FirstName!.StartsWith("B")),
                    "C" => query.Where(x => x.FirstName!.StartsWith("C")),
                    _ => query
                };
            }
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(x => x.FirstName),
                "email" => query.OrderBy(x => x.Email),
                "email_desc" => query.OrderByDescending(x => x.Email),
                _ => query.OrderBy(x => x.FirstName)
            };
            var totalUsers = await query.CountAsync();

            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (users, totalUsers);
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task CreateUserAsync(ApplicationUser user)
        {
            await _genericRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _genericRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _genericRepository.GetByIdAsync(id);

            if (user != null)
            {
                await _genericRepository.DeleteAsync(user);
            }
        }

    };
}
