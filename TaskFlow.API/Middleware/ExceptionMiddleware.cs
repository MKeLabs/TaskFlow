namespace TaskFlow.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            AccessViolationException => (StatusCodes.Status403Forbidden, "Acces interzis."),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resursa nu a fost gasita."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Argument invalid."),
            _ => (StatusCodes.Status500InternalServerError, "A aparut o eroare interna.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message
        });
    }
}
