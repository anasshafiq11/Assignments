using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Add New User
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email
            };
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

        }

        // Get User By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
                return NotFound();
            var response = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return  Ok(response);
        }

        // Update User By Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var user = new User
            {
                FirstName = updateUserDto.FirstName,
                LastName = updateUserDto.LastName,
                Email = updateUserDto.Email
            };
            var updatedUser = await _userRepository.UpdateUserAsync(id, user);
            return updatedUser == null ? NotFound() : Ok(updatedUser);
        }

        // Delete User By Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUser = await _userRepository.DeleteUserAsync(id);
            return deletedUser ? NoContent() : NotFound();
        }
    }
}