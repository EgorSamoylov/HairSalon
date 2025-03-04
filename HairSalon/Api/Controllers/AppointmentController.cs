using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService employeeService;
        public AppointmentController(IAppointmentService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var appointment = await employeeService.GetById(id);
            return Ok(appointment);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await employeeService.GetAll();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AppointmentDTO appointment)
        {
            await employeeService.Add(appointment);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AppointmentDTO appointment)
        {
            var result = await employeeService.Update(appointment);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await employeeService.Delete(id);
            return Ok(result);
        }
    }
}
