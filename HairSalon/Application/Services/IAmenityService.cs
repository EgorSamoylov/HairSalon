﻿using Application.DTOs;
using Application.Request.AmenityRequest;

namespace Application.Services
{
    public interface IAmenityService
    {
        public Task<AmenityDTO?> GetById(int id);
        public Task<IEnumerable<AmenityDTO>> GetAll();
        public Task Add(CreateAmenityRequest amenity);
        public Task<bool> Update(UpdateAmenityRequest amenity);
        public Task<bool> Delete(int id);
    }
}
