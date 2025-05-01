using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Services
{
    public interface IAuthEmplyeeService
    {
        string GenerateJwtToken(Employee user);
        Task<int> Register(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}
