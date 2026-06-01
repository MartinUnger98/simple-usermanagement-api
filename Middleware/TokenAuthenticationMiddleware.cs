using System.Text.Json;

namespace UserManagementAPI.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Allow Swagger endpoints without token so you can still test the API easily
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Protect only API endpoints
            if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    await WriteUnauthorizedResponse(context, "Missing Authorization header.");
                    return;
                }

                var authHeaderValue = authHeader.ToString();

                if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteUnauthorizedResponse(context, "Invalid Authorization header format.");
                    return;
                }

                var token = authHeaderValue["Bearer ".Length..].Trim();
                var validToken = _configuration["Authentication:ApiToken"];

                if (string.IsNullOrWhiteSpace(validToken) || token != validToken)
                {
                    await WriteUnauthorizedResponse(context, "Invalid token.");
                    return;
                }
            }

            await _next(context);
        }

        private static async Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Unauthorized",
                message = message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}