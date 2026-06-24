using Assignment2.ViewModels;
using Microsoft.AspNetCore.Identity;
using UserManagement.Common.Results;
using UserManagement.Services.Models.Account;

namespace UserManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResult> RegisterAsync(Register model);

        Task<OperationResult> LoginAsync(Login model);

        Task<OperationResult> ConfirmEmailAsync(string userId, string token);
        Task<OperationResult<string?>> GenerateConfirmationLinkAsync(string email);

        Task<OperationResult> InviteUserAsync(InviteUser model);

        Task<OperationResult> SetPasswordAsync(SetPassword model);

        Task LogoutAsync();
    }
}
