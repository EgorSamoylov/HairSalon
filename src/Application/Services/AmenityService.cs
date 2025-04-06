using Application.DTOs;
using Application.Exceptions;
using Application.Request.AmenityRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.AmenityRepository;

namespace Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private IMapper _mapper;

        public AmenityService(IAmenityRepository amenityRepository, IMapper mapper)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateAmenityRequest request)
        {
            var amenity = new Amenity()
            {
                ServiceName = request.ServiceName,
                Description = request.Description,
                AuthorId = request.AuthorId,
                Price = request.Price,
                DurationMinutes = request.DurationMinutes,
            };

            return await _amenityRepository.Create(amenity);
        }

        public async Task Delete(int id)
        {
            var result = await _amenityRepository.Delete(id);
            if (!result)
            {
                throw new EntityDeleteException("Amenity for deletion not found");
            }
        }

        public async Task<IEnumerable<AmenityDTO>> GetAll()
        {
            var amenities = await _amenityRepository.ReadAll();
            var mappedServices = amenities.Select(q => _mapper.Map<AmenityDTO>(q)).ToList();
            return mappedServices;
        }

        public async Task<AmenityDTO?> GetById(int id)
        {
            var amenity = await _amenityRepository.ReadById(id);
            if (amenity is null)
            {
                throw new NotFoundApplicationException("Amenity not found");
            }
            var mappedService = _mapper.Map<AmenityDTO>(amenity);
            return mappedService;
        }

        public async Task Update(UpdateAmenityRequest request)
        {
            var amenity = new Amenity()
            {
                ServiceName = request.ServiceName,
                Description = request.Description,
                AuthorId = request.AuthorId,
                Price = request.Price,
                DurationMinutes = request.DurationMinutes,
            };

            var result = await _amenityRepository.Update(amenity);
            if (!result)
            {
                throw new EntityUpdateException("Amenity wasn't updated");
            }
        }
    }
}
