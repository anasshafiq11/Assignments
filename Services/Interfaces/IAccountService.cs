using Assignment2.ViewModels;
using Microsoft.AspNetCore.Identity;
using UserManagement.DTOs.Account;

namespace UserManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);

        Task<SignInResult> LoginAsync(LoginDto dto);

        Task ConfirmEmailAsync(string userId, string token);

        Task<IdentityResult> InviteUserAsync(InviteUserDto dto);

        Task<IdentityResult> SetPasswordAsync(SetPasswordDto dto);

        Task LogoutAsync();
    }
}
