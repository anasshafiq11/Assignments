namespace Assignment2.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        // Create a logger specifically for the RequestLoggingMiddleware class.
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var user = context.User?.Identity?.Name ?? "Anonymous";

            _logger.LogInformation("Incoming request: {Method} | {Path} by {User}", method, path, user);

            await _next(context);
            _logger.LogInformation("Outgoing response: {StatusCode} for {Method} | {Path} by {User}", context.Response.StatusCode, method, path, user);
        }

    }
}
