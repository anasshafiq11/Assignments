using Assignment2.Models;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAppInfoService _appInfo;
        private readonly IRequestTracker _requestTracker;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, IAppInfoService appInfo, IRequestTracker requestTracker)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _appInfo = appInfo;
            _requestTracker = requestTracker;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index(string? search, string? sortOrder, string? filter, int page = 1)
        {
            // For DI
            ViewBag.ApplicationId = _appInfo.ApplicationId;
            ViewBag.StartTime = _appInfo.StartTime;
            ViewBag.RequestId = _requestTracker.RequestId;


            const int pageSize = 2;
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");
            var (users, totalRecords) = await _userService.GetUsersAsync(currentUserId, isAdmin,search, sortOrder, filter, page, pageSize);
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);
            var usersData = new UserListViewModel { Users = userViewModels, Search = search, SortOrder = sortOrder, CurrentPage = page, TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize) };
            return View(usersData);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = _mapper.Map<UserViewModel>(user);

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            _mapper.Map(model, user);

            user.UserName = user.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            //Ensure admin can only delete users they created
            var currentAdminId = _userManager.GetUserId(User);

            if (user.CreatedByAdminId != currentAdminId)
                return Forbid();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["Error"] += error.Description + " ";
                }
            }
            else
            {
                TempData["Success"] = "User deleted successfully.";
            }

            return RedirectToAction(nameof(Index));

        }

    }
}
