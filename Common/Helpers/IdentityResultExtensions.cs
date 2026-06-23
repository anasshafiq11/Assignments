using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserManagement.Common.Helpers
{
    public static class IdentityResultExtensions
    {
        public static void AddToModelState(this IdentityResult result,
            ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
