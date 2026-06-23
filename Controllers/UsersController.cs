using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Services.Interfaces;
using UsersApi.Common.Querying;
using UsersApi.Common.Helpers;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var createdUser = await _userService.AddUserAsync(user);

            var responseDto = _mapper.Map<UserResponseDto>(createdUser);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id },
                ApiResponseHelper.Success(responseDto, "User created successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] QueryOptions queryOptions)
        {
            var results = await _userService.GetAllUsersAsync(queryOptions);
            return Ok(ApiResponseHelper.Success(results, "Users retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponseHelper.Failure<UserResponseDto>("User not found"));
            }

            var responseDto = _mapper.Map<UserResponseDto>(user);

            return Ok(ApiResponseHelper.Success(responseDto, "User retrieved successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var user = _mapper.Map<User>(updateUserDto);

            var updatedUser = await _userService.UpdateUserAsync(id, user);

            if (updatedUser == null)
            {
                return NotFound(ApiResponseHelper.Failure<UserResponseDto>("User not found"));
            }

            var responseDto = _mapper.Map<UserResponseDto>(updatedUser);

            return Ok(ApiResponseHelper.Success(responseDto, "User updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userDeleted = await _userService.DeleteUserAsync(id);

            if (userDeleted == false)
            {
                return NotFound(ApiResponseHelper.Failure<object>("User not found"));
            }

            return Ok(ApiResponseHelper.Success(true, "User deleted successfully"));
        }
    }
}