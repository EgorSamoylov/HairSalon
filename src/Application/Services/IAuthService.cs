using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        Task<int> Register(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        public Task<int> Add(RegistrationRequest request);
    }
}
