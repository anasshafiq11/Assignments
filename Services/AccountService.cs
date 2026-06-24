using Assignment2.Models;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using UserManagement.Common.Constants.Auth;
using UserManagement.Common.Results;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Models.Account;

namespace UserManagement.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IEmailService emailService,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> RegisterAsync(Register model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return OperationResult.Failure(
                    result.Errors.Select(e => e.Description).ToArray());
            }

            await _userManager.AddToRoleAsync(user, Roles.Admin);

            return OperationResult.Success("Registration successful.");
        }

        public async Task<OperationResult> LoginAsync(Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email,
                model.Password, false, true);

            if (result.Succeeded)
                return OperationResult.Success();

            return OperationResult.Failure("Invalid email or password.");
        }

        public async Task<OperationResult<string?>> GenerateConfirmationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return OperationResult<string?>.Failure("User not found");


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var request = _httpContextAccessor.HttpContext!.Request;

            var link = $"{request.Scheme}://{request.Host}/Account/ConfirmEmail?userId={user.Id}&token={token}";

            return OperationResult<string?>.Success(link);
        }

        public async Task<OperationResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return OperationResult.Failure("User not found");

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            await _userManager.ConfirmEmailAsync(user, token);
            return OperationResult.Success("Email confirmed.");
        }

        public async Task<OperationResult> InviteUserAsync(InviteUser model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            user.UserName = model.Email;
            user.EmailConfirmed = true;
            user.CreatedByAdminId = model.AdminId;

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return OperationResult.Failure(
                    result.Errors.Select(e => e.Description).ToArray());
            }

            await _userManager.AddToRoleAsync(user, Roles.User);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var request = _httpContextAccessor.HttpContext!.Request;

            var link = $"{request.Scheme}://{request.Host}/Account/SetPassword?userId={user.Id}&token={token}";

            EmailMessage emailMessage = new EmailMessage { 
                ToEmail = user.Email, 
                Subject = "Account Invitation",
                Body = $"""Please click <a href="{link}">here</a> to set your password."""
            };

            await _emailService.SendEmailAsync(emailMessage);

            return OperationResult.Success("Registration successful.");
        }
        public async Task<OperationResult> SetPasswordAsync(SetPassword model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return OperationResult.Failure("User not found");

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

            if (!result.Succeeded)
            {
                return OperationResult.Failure(
                    result.Errors.Select(e => e.Description).ToArray());
            }

            return OperationResult.Success("Password set successfully.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
