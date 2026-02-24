using System.Net;
using System.Text.Json;

namespace Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing request {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new
            {
                message = "Unexpected error occurred.",
                traceId = context.TraceIdentifier
            });
            await context.Response.WriteAsync(payload);
        }
    }
}
