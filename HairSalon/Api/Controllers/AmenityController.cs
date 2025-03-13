using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AmenityController : ControllerBase
    {
        private IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenity = await _amenityService.GetById(id);
            
            if (amenity == null)
            {
                return NotFound();
            }
            
            return Ok(amenity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenity = await _amenityService.GetAll();
            return Ok(amenity);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AmenityDTO amenity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращает 400 Bad Request с информацией об ошибках валидации
            }

            var amenityId = await _amenityService.Add(amenity);
            var result = new { Id = amenityId };
            return CreatedAtAction(nameof(GetById), new { id = amenityId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AmenityDTO amenity)
        {
            var result = await _amenityService.Update(amenity);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _amenityService.Delete(id);
            
            if(!result)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
