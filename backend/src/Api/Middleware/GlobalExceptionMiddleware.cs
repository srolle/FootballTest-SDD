using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RequestValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while processing request {Path}", context.Request.Path);
            await WriteValidationProblemAsync(context, StatusCodes.Status400BadRequest, ex.Errors, "Validation failed");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument validation error while processing request {Path}", context.Request.Path);
            var errors = new Dictionary<string, string[]>
            {
                ["request"] = [ex.Message]
            };
            await WriteValidationProblemAsync(context, StatusCodes.Status400BadRequest, errors, "Validation failed");
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found while processing request {Path}", context.Request.Path);
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Resource not found", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing request {Path}", context.Request.Path);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Unexpected error", "Unexpected error occurred.");
        }
    }

    private static async Task WriteValidationProblemAsync(
        HttpContext context,
        int statusCode,
        IReadOnlyDictionary<string, string[]> errors,
        string title)
    {
        var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>(errors))
        {
            Status = statusCode,
            Title = title,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = statusCode switch
            {
                StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                StatusCodes.Status500InternalServerError => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                _ => "https://tools.ietf.org/html/rfc9110"
            }
        };
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
