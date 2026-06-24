using Assignment2.Common.Querying;
using Assignment2.Models;
using Assignment2.Repositories.Interfaces;
using Assignment2.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common.Results;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Models.User;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Assignment2.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IGenericRepository<ApplicationUser> _repository;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICurrentUserService currentUser,
            IGenericRepository<ApplicationUser> repository,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        

        public async Task<OperationResult<UserList>> GetUsersAsync(QueryOptions queryOptions)
        {
            var users = await _repository.GetPagedAndFilteredAsync(queryOptions);
            users = users.Where(u => u.CreatedByAdminId == _currentUser.UserId).ToList();
            var result = new UserList
            {
                Users = _mapper.Map<List<User>>(users),

                Search = queryOptions.FilterExpression,

                SortOrder = queryOptions.OrderByExpression,

                CurrentPage = (queryOptions.Skip / queryOptions.Take) + 1,

                TotalPages = (int)Math.Ceiling((double)users.Count / queryOptions.Take)
            };
            return OperationResult<UserList>.Success(result);
        }
        public async Task<OperationResult<User>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return OperationResult<User>.Failure("User not found.");
            }

            return OperationResult<User>.Success(_mapper.Map<User>(user));
        }

        public async Task<OperationResult> CreateUserAsync(User user)
        {
            var appUser = _mapper.Map<ApplicationUser>(user);
            await _repository.AddAsync(appUser);
            return OperationResult.Success("User created successfully");
        }

        public async Task<OperationResult<User>> UpdateUserAsync(UpdateUser dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
            {
                return OperationResult<User>.Failure("User not found.");
            }

            _mapper.Map(dto, user);
            user.UserName = user.Email;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return OperationResult<User>.Failure(result.Errors
                        .Select(x => x.Description).ToArray());
            }

            return OperationResult<User>.Success(
                _mapper.Map<User>(user), "User updated successfully.");
        }

        public async Task<OperationResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return OperationResult.Failure("User not found.");
            }

            if (user.CreatedByAdminId != _currentUser.UserId)
            {
                return OperationResult.Failure("You can only delete users created by you.");
            }

            await _userManager.UpdateSecurityStampAsync(user);

            if (user.Id == _currentUser.UserId)
            {
                await _signInManager.SignOutAsync();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return OperationResult.Failure(result.Errors.Select(e => e.Description).ToArray());
            }

            return OperationResult.Success("User deleted successfully.");
        }

    };
}

