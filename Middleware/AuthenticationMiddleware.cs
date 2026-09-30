using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly string _apiKey;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _apiKey = configuration["ApiSettings:ApiKey"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only protect API endpoints
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaders))
            {
                _logger.LogWarning("Missing Authorization header for {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var header = authHeaders.ToString();
            if (!header.StartsWith("Bearer "))
            {
                _logger.LogWarning("Invalid Authorization header format for {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var token = header.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(_apiKey) || token != _apiKey)
            {
                _logger.LogWarning("Invalid token for {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}
