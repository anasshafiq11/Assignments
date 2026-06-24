using Assignment2.Common.Querying;
using Assignment2.Services.Interfaces;
using Assignment2.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.Constants.Auth;
using UserManagement.Common.Helpers;
using UserManagement.Services.Models.User;

namespace Assignment2.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(QueryOptions queryOptions)
        {
            var result = await _userService.GetUsersAsync(queryOptions);

            if (!result.Succeeded)
            {
                return View(new UserListViewModel());
            }

            var model = _mapper.Map<UserListViewModel>(result.Data);

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            var model = _mapper.Map<UserViewModel>(result.Data);

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = _mapper.Map<UpdateUser>(model);

            var result = await _userService.UpdateUserAsync(dto);

            if (!result.Succeeded)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            TempData["Success"] = result.Message;

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
             //   TempData["Error"] = string.Join(", ", result.Errors);
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}