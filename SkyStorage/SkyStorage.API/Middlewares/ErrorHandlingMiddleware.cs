using SkyStorage.Domain.Exceptions;

namespace SkyStorage.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, 404, ex, ex.Message);
        }
        catch (MismatchException ex)
        {
            await HandleExceptionAsync(context, 403, ex, ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            await HandleExceptionAsync(context, 400, ex, ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, 500, ex, "Something went wrong");
        }
    }

    private Task HandleExceptionAsync(HttpContext context, int statusCode, Exception ex, string message)
    {
        logger.LogError(ex, message);
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(message);
    }
}
