public class IPLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IPLoggingMiddleware> _logger;

    public IPLoggingMiddleware(RequestDelegate next, ILogger<IPLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var requestPath = context.Request.Path;

        _logger.LogInformation("Incoming request from IP: {IPAddress}, Path: {RequestPath}", ipAddress, requestPath);

        await _next(context);
    }
}