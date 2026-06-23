using Assignment2.Models;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using UserManagement.Common.Constants.Auth;
using UserManagement.DTOs.Account;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);

            var result = await _userManager.CreateAsync(
                user,
                dto.Password);

            if (!result.Succeeded)
                return result;

            await _userManager.AddToRoleAsync(
                user,
                Roles.User);

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            return await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                false,
                true);
        }

        public async Task<(bool Success, string? Link)>GenerateConfirmationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return (false, null);

            var token =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var request = _httpContextAccessor.HttpContext!.Request;

            var link =
                $"{request.Scheme}://{request.Host}/Account/ConfirmEmail?userId={user.Id}&token={token}";

            return (true, link);
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> InviteUserAsync(
     InviteUserDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);

            user.UserName = dto.Email;
            user.EmailConfirmed = true;
            user.CreatedByAdminId = dto.AdminId;

            var result =
                await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return result;

            await _userManager.AddToRoleAsync(
                user,
                Roles.User);

            var token =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var request =
                _httpContextAccessor.HttpContext!.Request;

            var link =
                $"{request.Scheme}://{request.Host}/Account/SetPassword?userId={user.Id}&token={token}";

            await _emailService.SendEmailAsync(
                user.Email!,
                "Account Invitation",
                $"Please click <a href='{link}'>here</a> to set your password.");

            return result;
        }
        public async Task<IdentityResult> SetPasswordAsync(SetPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            return await _userManager.ResetPasswordAsync(user, token, model.Password);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
