using Assignment2.Common.Querying;
using Assignment2.Models;
using Assignment2.Repositories.Interfaces;
using Assignment2.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs.User;
using UserManagement.Services.Interfaces;

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

        

        public async Task<UserListDto> GetUsersAsync(QueryOptions queryOptions)
        {
            var users = await _repository.GetPagedAndFilteredAsync(queryOptions);

            return new UserListDto
            {
                Users = _mapper.Map<List<UserDto>>(users),

                Search = queryOptions.FilterExpression,

                SortOrder = queryOptions.OrderByExpression,

                CurrentPage = (queryOptions.Skip / queryOptions.Take) + 1,

                TotalPages = (int)Math.Ceiling((double)users.Count / queryOptions.Take)
            };
        }
        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task CreateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            await _repository.AddAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto dto)
        {
            var user =  await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "User not found." });
            }

            _mapper.Map(dto, user);

            user.UserName = user.Email;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<DeleteUserResultDto> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new DeleteUserResultDto
                { Succeeded = false, ErrorMessage = "User not found." };
            }

            if (user.CreatedByAdminId != _currentUser.UserId)
            {
                return new DeleteUserResultDto
                {
                    Succeeded = false,
                    ErrorMessage = "You can only delete users created by you."
                };
            }

            await _userManager.UpdateSecurityStampAsync(user);

            bool selfDeleted = user.Id == _currentUser.UserId;

            if (selfDeleted)
            {
                await _signInManager.SignOutAsync();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new DeleteUserResultDto
                {
                    Succeeded = false,
                    ErrorMessage = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description))
                };
            }

            return new DeleteUserResultDto
            {
                Succeeded = true,
                SelfDeleted = selfDeleted
            };
        }

    };
}

