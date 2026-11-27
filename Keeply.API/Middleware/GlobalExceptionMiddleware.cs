using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.HasStarted)
                return;

            if (context.Response.StatusCode >= 400)
                await WriteProblemAsync(context, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            if (context.Response.HasStarted)
                return;

            var detail = context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment() ? ex.ToString() : null;
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.", detail);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string? title = null, string? detail = null)
    {
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title ?? GetTitle(statusCode),
            Detail = detail
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad request.",
        StatusCodes.Status401Unauthorized => "Unauthorized.",
        StatusCodes.Status403Forbidden => "Forbidden.",
        StatusCodes.Status404NotFound => "Resource not found.",
        StatusCodes.Status409Conflict => "Conflict.",
        _ => "An error occurred."
    };
}
