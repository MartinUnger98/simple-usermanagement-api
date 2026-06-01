namespace UserManagementAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                "Incoming Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            _logger.LogInformation(
                "Outgoing Response: {StatusCode}",
                context.Response.StatusCode);
        }
    }
}