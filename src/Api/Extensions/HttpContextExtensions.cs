using Domain.Entities;

namespace Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static UserContext GetUserContext(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue("UserContext", out var context) &&
                context is UserContext userContext)
            {
                return userContext;
            }

            throw new UnauthorizedAccessException("User context not found");
        }
    }
}
