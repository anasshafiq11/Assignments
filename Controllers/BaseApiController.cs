using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Common.Helpers;

namespace UsersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult CreatedResponse<T>(string actionName, object routeValues,
            T data,string message = "")
        {
            return CreatedAtAction(actionName, routeValues, ApiResponseHelper.Success(data, message));
        }
        protected IActionResult Success<T>(T data, string message = "")
        {
            return Ok(ApiResponseHelper.Success(data, message));
        }

        protected IActionResult Failure(string message)
        {
            return BadRequest(ApiResponseHelper.Failure<object>(message));
        }

        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(ApiResponseHelper.Failure<object>(message));
        }

        protected IActionResult UnAuthorized(string message)
        {
            return Unauthorized(ApiResponseHelper.Failure<string>(message));
        }
    }
}
