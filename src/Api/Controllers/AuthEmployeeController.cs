using Application.Request;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthEmployeeController(IAuthEmployeeService authService) : ControllerBase
    {
        [EnableRateLimiting("register")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            await authService.Register(request);
            return Created();
        }

        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.Request.LoginRequest request)
        {
            var response = await authService.Login(request);
            return Ok(response);
        }
    }
}
