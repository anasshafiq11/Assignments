using Assignment2.Common.Querying;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.Constants.Auth;
using UserManagement.DTOs.User;

namespace Assignment2.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAppInfoService _appInfo;
        private readonly IRequestTracker _requestTracker;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IAppInfoService appInfo,
            IRequestTracker requestTracker, IMapper mapper)
        {
            _userService = userService;
            _appInfo = appInfo;
            _requestTracker = requestTracker;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(QueryOptions queryOptions)
        {
            ViewBag.ApplicationId = _appInfo.ApplicationId;
            ViewBag.StartTime = _appInfo.StartTime;
            ViewBag.RequestId = _requestTracker.RequestId;

            var dto = await _userService.GetUsersAsync(queryOptions);

            var model = _mapper.Map<UserListViewModel>(dto);

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var dto = await _userService.GetUserByIdAsync(id);

            if (dto == null)
                return NotFound();

            var model = _mapper.Map<UserViewModel>(dto);

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = _mapper.Map<UpdateUserDto>(model);

            var result = await _userService.UpdateUserAsync(dto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            TempData["Success"] = "User updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.Succeeded)
            {
                TempData["Error"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "User deleted successfully.";

            if (result.SelfDeleted)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}