using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace UserManagementAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

            await _next(context);

            sw.Stop();
            var status = context.Response?.StatusCode;
            _logger.LogInformation("Outgoing response: {Method} {Path} responded {StatusCode} in {Elapsed}ms",
                context.Request.Method, context.Request.Path, status, sw.ElapsedMilliseconds);
        }
    }
}
