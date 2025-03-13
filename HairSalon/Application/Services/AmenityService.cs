using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private IMapper _mapper;

        public AmenityService (IAmenityRepository amenityRepository, IMapper mapper)
        {
            this._amenityRepository = amenityRepository;
            this._mapper = mapper;
        }

        public async Task<int> Add(AmenityDTO amenity)
        {
            var mappedService = _mapper.Map<Amenity>(amenity);
            if (mappedService != null)
            {
                await _amenityRepository.Create(mappedService);
                return mappedService.Id;
            }

            throw new ArgumentException("Failed to map AmenityDTO to Amenity"); //Или return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _amenityRepository.Delete(id);
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
            var mappedService = _mapper.Map<AmenityDTO>(amenity);
            return mappedService;
        }

        public async Task<bool> Update(AmenityDTO amenity)
        {
            var mappedService = _mapper.Map<Amenity>(amenity);
            return await _amenityRepository.Update(mappedService);
        }
    }
}
