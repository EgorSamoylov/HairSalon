using Application.Request;
using System.Security.Claims;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> Register(RegistrationRequest request);
        Task<ClaimsPrincipal> Login(LoginRequest request);
    }
}
