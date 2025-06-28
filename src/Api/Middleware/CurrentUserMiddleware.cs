using Domain.Entities;

namespace Api.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userContext = new UserContext();

            // В реальном приложении берем из JWT-токена
            if (context.Request.Headers.TryGetValue("X-User-Id", out var userId) &&
                int.TryParse(userId, out var id))
            {
                userContext.UserId = id;
            }

            if (context.Request.Headers.TryGetValue("X-User-Role", out var role))
            {
                userContext.Role = role;
            }

            context.Items["UserContext"] = userContext;
            await _next(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
}
