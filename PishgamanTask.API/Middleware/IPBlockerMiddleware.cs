using System.Net;

namespace PishgamanTask.API.Middleware
{
    public class IPBlockerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        public IEnumerable<string>? blockedIps;

        public IPBlockerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<IPBlockerMiddleware>();
            Configuration = configuration;

            blockedIps = Configuration.GetSection("BlockedIPAddresses").Get<string[]>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (blockedIps != null)
                    if (blockedIps.Contains(context.Connection.RemoteIpAddress?.ToString()))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        await context.Response.WriteAsync("Sorry! Your IP Address is restricted by administrator!");
                    }

                await _next(context);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
            }
        }
    }
}
