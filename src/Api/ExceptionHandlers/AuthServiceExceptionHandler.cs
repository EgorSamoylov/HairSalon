using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandlers
{
    public class AuthServiceExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
             HttpContext httpContext,
             Exception exception,
             CancellationToken cancellationToken)
        {
            // Проверяем, является ли исключение UnauthorizedAccessException
            if (exception is UnauthorizedAccessException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = "Invalid email or password",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };

                httpContext.Response.StatusCode = problemDetails.Status.Value;

                // Используем ProblemDetailsService для стандартизированного ответа
                await problemDetailsService.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = exception
                });

                return true;
            }

            // Если исключение другого типа - пропускаем для обработки другими обработчиками
            return false;
        }
    }
}
