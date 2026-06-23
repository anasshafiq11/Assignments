using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Common.Helpers;
using UsersApi.DTOs;
using UsersApi.Services.Interfaces;

namespace UsersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.AuthenticateAsync(dto);
            if(token == null)
            {
                return Unauthorized(ApiResponseHelper.Failure<string>("Invalid email or password"));
            }
            return Ok(ApiResponseHelper.Success(token, "Authentication successful"));
        }

}
}
