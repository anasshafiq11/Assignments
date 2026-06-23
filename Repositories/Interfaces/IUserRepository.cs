using UsersApi.Models;

namespace UsersApi.Repositories.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);

    }
}
