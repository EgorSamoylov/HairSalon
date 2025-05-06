using Api.Extensions;
using Application.Request.ClientRequest;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService clientService)
        {
            _userService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("userInfo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return NotFound();
            }
            var user = await _userService.GetById(userId.Value);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.Add(request);
            var result = new { Id = userId };
            return CreatedAtAction(nameof(GetById), result, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            await _userService.Update(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
    }
}
