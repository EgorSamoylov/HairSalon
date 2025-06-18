using Application.Request;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> Register(RegistrationRequest request);
        Task<ClaimsPrincipal> Login(LoginRequest request);
        Task<ClaimsPrincipal> RegisterEmployee([FromBody] RegistrationRequest request);
    }
}
