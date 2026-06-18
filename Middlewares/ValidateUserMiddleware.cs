using Assignment2.Models;
using Microsoft.AspNetCore.Identity;

namespace Assignment2.Middlewares
{
    public class ValidateUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // we do method injection here 
        // because userManager and signInManager are scoped services and scoped service is tied to a request, while the middleware instance is created once and reused.
        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = userManager.GetUserId(context.User);

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);

                    if (user == null)
                    {
                        await signInManager.SignOutAsync();

                        context.Response.Redirect("/Account/Login");

                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}