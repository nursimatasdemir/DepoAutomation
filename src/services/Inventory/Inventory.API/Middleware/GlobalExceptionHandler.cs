using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Bir hata yakalandı: {Message}", exception.Message);

        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationException.Errors
                .GroupBy(
                    e => e.PropertyName,
                    e => e.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName, Values = errorMessages.ToArray()
                    })
                .ToDictionary(
                    x => x.Key,
                    x => x.Values
                );
            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bir veya daha fazla doğrulama hatası oluştu.",
            };
            
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
        return false;
    }
}
