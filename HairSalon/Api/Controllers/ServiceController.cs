using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private IServiceService serviceService;
        public ServiceController(IServiceService serviceService)
        {
            this.serviceService = serviceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var service = await serviceService.GetById(id);
            return Ok(service);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await serviceService.GetAll();
            return Ok(services);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ServiceDTO service)
        {
            await serviceService.Add(service);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ServiceDTO service)
        {
            var result = await serviceService.Update(service);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await serviceService.Delete(id);
            return Ok(result);
        }
    }
}
