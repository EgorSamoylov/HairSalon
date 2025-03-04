using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var employee = await employeeService.GetById(id);
            return Ok(employee);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await employeeService.GetAll();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeDTO employee)
        {
            await employeeService.Add(employee);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeDTO employee)
        {
            var result = await employeeService.Update(employee);
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
