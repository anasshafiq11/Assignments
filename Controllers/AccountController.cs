using Assignment2.Models;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.Constants.Auth;
using UserManagement.Common.Helpers;
using UserManagement.DTOs.Account;
using UserManagement.Services.Interfaces;

namespace Assignment2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper)
        {
            _accountService = accountService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerDto = _mapper.Map<RegisterDto>(model);

            var result = await _accountService.RegisterAsync(registerDto);

            if (!result.Succeeded)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            return RedirectToAction(nameof(RegistrationSuccess));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

       

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = _mapper.Map<LoginDto>(model);

            var result = await _accountService.LoginAsync(loginDto);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            return RedirectToAction("Index", "User");
        }

        

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                await _accountService.ConfirmEmailAsync(userId, token);
                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }

       

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public IActionResult InviteUser()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteUser(InviteUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = _mapper.Map<InviteUserDto>(model);

            dto.AdminId = _currentUserService.UserId!;

            var result =  await _accountService.InviteUserAsync(dto);

            if (!result.Succeeded)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            TempData["Success"] = "Invitation email sent successfully.";

            return RedirectToAction(nameof(InviteUser));
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult SetPassword(string userId, string token)
        {
            return View(new SetPasswordViewModel{ UserId = userId, Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var setPasswordDto = _mapper.Map<SetPasswordDto>(model);

            var result = await _accountService.SetPasswordAsync(setPasswordDto);

            if (!result.Succeeded)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            TempData["Success"] = "Password set successfully.";

            return RedirectToAction(nameof(Login));
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}