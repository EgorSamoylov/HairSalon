using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Services
{
    public interface IAuthClientService
    {
        string GenerateJwtToken(Client user);
        Task<int> Register(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}
