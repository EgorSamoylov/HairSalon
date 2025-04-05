using Application.DTOs;
using Application.Request.AppointmentRequest;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService employeeService)
        {
            _appointmentService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetById(id);
            return Ok(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentService.GetAll();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAppointmentRequest request)
        {
            var appointmentId = await _appointmentService.Add(request);
            var result = new { Id = appointmentId };
            return CreatedAtAction(nameof(GetById), new { id = appointmentId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAppointmentRequest request)
        {
            var result = await _appointmentService.Update(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentService.Delete(id);

            if (!result) 
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
