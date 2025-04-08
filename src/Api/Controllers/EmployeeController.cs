using Application.Request.EmployeeRequest;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetById(id);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeRequest request)
        {
            var employeeId = await _employeeService.Add(request);
            var result = new { Id = employeeId };
            return CreatedAtAction(nameof(GetById), result, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest request)
        {
            await _employeeService.Update(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.Delete(id);
            return NoContent();
        }
    }
}
