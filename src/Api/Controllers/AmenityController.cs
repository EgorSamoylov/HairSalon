using Application.Request.AmenityRequest;
using Application.Services;
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
            return Ok(amenity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenity = await _amenityService.GetAll();
            return Ok(amenity);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAmenityRequest request)
        {
            var amenityId = await _amenityService.Add(request);
            var result = new { Id = amenityId };
            return CreatedAtAction(nameof(GetById), result, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAmenityRequest request)
        {
            await _amenityService.Update(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _amenityService.Delete(id);
            return NoContent();
        }
    }
}
