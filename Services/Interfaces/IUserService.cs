using Assignment2.Common.Querying;
using Assignment2.Models;
using Assignment2.ViewModels;
using Microsoft.AspNetCore.Identity;
using UserManagement.DTOs.User;

namespace Assignment2.Services.Interfaces
{
    public interface IUserService
    {

        Task<UserListDto> GetUsersAsync(QueryOptions queryOptions);
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(UpdateUserDto dto);
        Task CreateUserAsync(UserDto user);
        Task<DeleteUserResultDto> DeleteUserAsync(string id);

    }
}
