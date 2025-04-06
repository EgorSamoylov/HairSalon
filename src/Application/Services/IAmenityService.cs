using Application.DTOs;
using Application.Request.AmenityRequest;

namespace Application.Services
{
    public interface IAmenityService
    {
        public Task<AmenityDTO?> GetById(int id);
        public Task<IEnumerable<AmenityDTO>> GetAll();
        public Task<int> Add(CreateAmenityRequest amenity);
        public Task Update(UpdateAmenityRequest amenity);
        public Task Delete(int id);
    }
}
