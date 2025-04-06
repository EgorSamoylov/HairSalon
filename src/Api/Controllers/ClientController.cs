using Application.Request.ClientRequest;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetById(id);
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAll();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateClientRequest request)
        {
            var clientId = await _clientService.Add(request);
            var result = new { Id = clientId };
            return CreatedAtAction(nameof(GetById), result, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientRequest request)
        {
            await _clientService.Update(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.Delete(id);
            return NoContent();
        }
    }
}
