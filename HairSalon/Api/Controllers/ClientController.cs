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
        private IClientService clientService;
        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var client = await clientService.GetById(id);
            return Ok(client);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await clientService.GetAll();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientDTO client)
        {
            await clientService.Add(client);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientDTO client)
        {
            var result = await clientService.Update(client);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await clientService.Delete(id);
            return Ok(result);
        }
    }
}
