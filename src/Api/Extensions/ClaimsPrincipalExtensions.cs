using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : null;
        }


        // Новый метод для получения роли пользователя
        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
