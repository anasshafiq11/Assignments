using UsersApi.DTOs;
using UsersApi.Repositories.Interfaces;
using UsersApi.Services.Interfaces;

namespace UsersApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<string?> AuthenticateAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return null;

            if (user.Password != dto.Password)
                return null;

            return _jwtService.GenerateToken(user);
        }

    }
}
