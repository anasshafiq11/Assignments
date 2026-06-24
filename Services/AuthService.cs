using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;
using UsersApi.Services.Interfaces;
using UsersApi.Services.Models;

namespace UsersApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IJwtService _jwtService;

        public AuthService(IGenericRepository<User> repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }
        public async Task<string?> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _repository.GetAsync(u => u.Email == loginRequest.Email);

            if (user == null)
                return null;

            if (user.Password != loginRequest.Password)
                return null;

            return _jwtService.GenerateToken(user);
        }

    }
}
