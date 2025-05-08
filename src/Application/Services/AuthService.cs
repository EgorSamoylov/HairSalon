using Application.Request;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService(
        IConfiguration configuration,
        IMapper mapper,
        IUserRepository userRepository,
        IBCryptHasher hasher) : IAuthService
    {
        public async Task<int> Register(RegistrationRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);
            user.Role = UserRoles.User;

            var userId = await userRepository.Create(user);

            return userId;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await userRepository.ReadByEmail(request.Email);
            var passwordVerified = hasher.VerifyPassword(request.Password, user?.PasswordHash);
            if (user == null || !passwordVerified || user?.PasswordHash == null)
            {
                throw new UnauthorizedAccessException();
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse() { Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSecret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var jwtIssuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
            var jwtAudience = configuration["JwtSettings:Audience"] ??
                              throw new ArgumentNullException("JwtSettings:Audience");
            var jwtExpirationMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
