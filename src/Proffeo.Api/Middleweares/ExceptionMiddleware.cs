using System.Text.Json;
using Npgsql;
using Proffeo.Models.Exceptions;

namespace Proffeo.Api.Middleweares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception e) { await HandleExceptionAsync(context, e); }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, details) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Data validation error", exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", exception.Message),
            NpgsqlException or InvalidOperationException when IsDatabaseError(exception) 
                => (StatusCodes.Status503ServiceUnavailable, "Database connection error", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", exception.Message)
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(
            new ErrorResponse(message, details)
        ));
    }

    private static bool IsDatabaseError(Exception ex) =>
        ex is NpgsqlException || (ex is InvalidOperationException && (ex.Source?.ToLower().Contains("sql") ?? false));
    
    private record ErrorResponse(string Message, string Details);
}