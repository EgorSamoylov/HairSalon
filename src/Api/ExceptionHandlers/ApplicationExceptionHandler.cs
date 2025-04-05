using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandlers
{
    public class ApplicationExceptionHandler
        (IProblemDetailsService _problemDetailsService)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BaseApplicationException e)
                return false;

            httpContext.Response.StatusCode = (int)e.StatusCode;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = (int)e.StatusCode,
                Title = e.Title,
                Detail = e.Message,
                Instance = httpContext.Request.Path,
                Type = e.GetType().Name
            };

            var problemDetailsContext = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = e
            };

            await _problemDetailsService.WriteAsync(problemDetailsContext);

            return true;
        }
    }
}
