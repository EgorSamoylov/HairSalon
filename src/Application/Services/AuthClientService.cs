using Application.Request;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class AuthClientService(
         IConfiguration configuration,
         IMapper mapper,
         IClientRepository userRepository,
         IPasswordHasher hasher
     ) : IAuthClientService
    {
        public async Task<int> Register(RegistrationRequest request)
        {
            var user = mapper.Map<Client>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);

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
        public string GenerateJwtToken(Client user)
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
                 new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                 new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                 new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
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
