
using UsersApi.Services.Models;

namespace UsersApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(LoginRequest loginRequest);
    }
}
