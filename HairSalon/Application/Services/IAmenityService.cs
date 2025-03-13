using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAmenityService
    {
        public Task<AmenityDTO?> GetById(int id);
        public Task<IEnumerable<AmenityDTO>> GetAll();
        public Task<int> Add(AmenityDTO amenity);
        public Task<bool> Update(AmenityDTO amenity);
        public Task<bool> Delete(int id);
    }
}
