using Assignment2.Models;
using Assignment2.Services;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _mapper = mapper;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await _userManager.AddToRoleAsync(user, "Admin");

                var confirmationLink = Url.Action("ConfirmEmail","Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                TempData["ConfirmationLink"] = confirmationLink;

                return RedirectToAction(nameof(RegistrationSuccess));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            return View("Error");
        }
        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Login");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var token =
                    await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id,token = token }, Request.Scheme);

                TempData["ConfirmationLink"] = confirmationLink;

                return RedirectToAction(nameof(RegistrationSuccess));
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }

            ModelState.AddModelError("", "Invalid Login Attempt");

            return View(model);
        }
        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> GenerateToken(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            var token = await _jwtService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult InviteUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> InviteUser(InviteUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                return View(model);
            }
            var adminId = _userManager.GetUserId(User);

           
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = user.Email;
            user.EmailConfirmed = true;
            user.CreatedByAdminId = adminId;

            var result = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("SetPassword", "Account", new { userId = user.Id, token = token }, Request.Scheme);

            try
            {
                await _emailService.SendEmailAsync(user.Email,"Account Invitation",
                    $"Please click <a href='{link}'>here</a> to set your password.");

                TempData["Success"] = "User created and invitation email sent.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(InviteUser));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SetPassword(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            if (await _userManager.HasPasswordAsync(user))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new SetPasswordViewModel { UserId = userId, Token = token });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return View("Error");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}