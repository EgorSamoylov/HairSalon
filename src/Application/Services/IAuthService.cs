using Application.Request;
using Application.Response;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<int> Register(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}
