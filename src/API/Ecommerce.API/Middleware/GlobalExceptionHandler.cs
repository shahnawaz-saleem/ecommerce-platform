using Microsoft.AspNetCore.Diagnostics;
using System.Net;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred");

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            error = exception.Message
        });

        return true;
    }
}