using Application.DTOs;
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
            
            if (employee == null)
            {
                return NotFound();
            }
            
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращает 400 Bad Request с информацией об ошибках валидации
            }

            var employeeId = await _employeeService.Add(employee);
            var result = new { Id = employeeId };
            return CreatedAtAction(nameof(GetById), new { id = employeeId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeDTO employee)
        {
            var result = await _employeeService.Update(employee);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.Delete(id);
            
            if (!result)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
