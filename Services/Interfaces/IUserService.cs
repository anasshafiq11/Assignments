using Assignment2.Common.Querying;
using Assignment2.Models;
using Assignment2.ViewModels;
using Microsoft.AspNetCore.Identity;
using UserManagement.Common.Results;
using UserManagement.Services.Models.User;

namespace Assignment2.Services.Interfaces
{
    public interface IUserService
    {

        Task<OperationResult<UserList>> GetUsersAsync(QueryOptions queryOptions);

        Task<OperationResult<User>> GetUserByIdAsync(string id);

        Task<OperationResult> CreateUserAsync(User user);

        Task<OperationResult<User>> UpdateUserAsync(UpdateUser dto);

        Task<OperationResult> DeleteUserAsync(string id);

    }
}
