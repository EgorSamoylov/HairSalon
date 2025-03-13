using Application.DTOs;
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
            this._clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetById(id);
            
            if (client == null)
            {
                return NotFound();
            }
            
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAll();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientDTO client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращает 400 Bad Request с информацией об ошибках валидации
            }

            var clientId = await _clientService.Add(client);
            var result =  new { Id = clientId };
            return CreatedAtAction(nameof(GetById), new { id = clientId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientDTO client)
        {
            var result = await _clientService.Update(client);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clientService.Delete(id);
            
            if (!result)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
