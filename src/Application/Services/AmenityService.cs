using Application.DTOs;
using Application.Exceptions;
using Application.Request.AmenityRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.AmenityRepository;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private IMapper _mapper;
        private readonly ILogger<AmenityService> _logger;

        public AmenityService(IAmenityRepository amenityRepository,
            IMapper mapper,
            ILogger<AmenityService> logger)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _logger = logger;
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

            int amenityId = await _amenityRepository.Create(amenity);
            _logger.LogInformation(
                @"Amenity created with id {Id} with ServiceName {ServiceName}, 
                Description {Description},
                by author {AuthorId},
                with Price {Price},
                DurationMinutes {DurationMinutes}",
                amenityId,
                amenity.ServiceName,
                amenity.Description,
                amenity.AuthorId,
                amenity.Price,
                amenity.DurationMinutes);
            return amenityId;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete amenity with id {Id}", id);

            var result = await _amenityRepository.Delete(id);
            if (!result)
            {
                _logger.LogWarning("Amenity with id {Id} not found for deletion", id);
                throw new EntityDeleteException("Amenity for deletion not found");
            }

            _logger.LogInformation("Amenity with id {Id} successfully deleted", id);
        }

        public async Task<IEnumerable<AmenityDTO>> GetAll()
        {
            _logger.LogInformation("Getting all amenities");

            var amenities = await _amenityRepository.ReadAll();
            var mappedServices = amenities.Select(q => _mapper.Map<AmenityDTO>(q));

            _logger.LogInformation("Retrieved {Count} amenities", mappedServices.Count());
            return mappedServices;
        }

        public async Task<AmenityDTO> GetById(int id)
        {
            _logger.LogInformation("Getting amenity by id {Id}", id);

            var amenity = await _amenityRepository.ReadById(id);
            if (amenity is null)
            {
                _logger.LogWarning("Amenity with id {Id} not found", id);
                throw new NotFoundApplicationException("Amenity not found");
            }

            var mappedService = _mapper.Map<AmenityDTO>(amenity);

            _logger.LogInformation(
                @"Retrieved amenity with id {Id}: ServiceName {ServiceName}, 
                Description {Description},
                AuthorId {AuthorId},
                Price {Price},
                DurationMinutes {DurationMinutes}",
                id,
                amenity.ServiceName,
                amenity.Description,
                amenity.AuthorId,
                amenity.Price,
                amenity.DurationMinutes);

            return mappedService;
        }

        public async Task Update(UpdateAmenityRequest request)
        {
            _logger.LogInformation(
                @"Attempting to update amenity with id {Id}: ServiceName {ServiceName}, 
                Description {Description},
                AuthorId {AuthorId},
                Price {Price},
                DurationMinutes {DurationMinutes}",
                request.AmenityId,
                request.ServiceName,
                request.Description,
                request.AuthorId,
                request.Price,
                request.DurationMinutes);

            var amenity = new Amenity()
            {
                Id = request.AmenityId,
                ServiceName = request.ServiceName,
                Description = request.Description,
                AuthorId = request.AuthorId,
                Price = request.Price,
                DurationMinutes = request.DurationMinutes,
            };

            var result = await _amenityRepository.Update(amenity);
            if (!result)
            {
                _logger.LogWarning("Amenity with id {Id} not found for update", request.AmenityId);
                throw new EntityUpdateException("Amenity wasn't updated");
            }

            _logger.LogInformation("Amenity with id {Id} successfully updated", request.AmenityId);
        }
    }
}
