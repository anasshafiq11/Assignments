using UsersApi.Common.Responses;

namespace UsersApi.Common.Helpers
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<T> Success<T>(T data, string? message = null)
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }
        public static ApiResponse<T> Failure<T>(string? error)
        {
            return new ApiResponse<T> { Success = false, Error = error, Data = default };
        }
    }
}
