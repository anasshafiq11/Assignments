using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UserManagement.Common.Results;

namespace UserManagement.Common.Helpers
{
    public static class OperationResultExtensions
    {
        public static void AddToModelState(this OperationResult result,
            ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError("", error);
            }
        }
    }
}
