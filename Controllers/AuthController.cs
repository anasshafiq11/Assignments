using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Common.Helpers;
using UsersApi.DTOs.Auth;
using UsersApi.Services.Interfaces;
using UsersApi.Services.Models;

namespace UsersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            LoginRequest loginRequest = _mapper.Map<LoginRequest>(dto);
            var token = await _authService.AuthenticateAsync(loginRequest);
            if(token == null)
            {
                return UnAuthorized("Invalid login attempt");
            }
            return Success(token, "Authentication successful");
        }

}
}
