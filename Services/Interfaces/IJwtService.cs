using UsersApi.Models;

namespace UsersApi.Services.Interfaces
{
    public interface IJwtService
    {

        string GenerateToken(User user);
    }
}
