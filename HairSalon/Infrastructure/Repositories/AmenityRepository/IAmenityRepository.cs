using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AmenityRepository
{
    public interface IAmenityRepository
    {
        public Task<Amenity?> ReadById(int id);
        public Task<List<Amenity>> ReadAll();
        public Task<int> Create(Amenity service);
        public Task<bool> Update(Amenity service);
        public Task<bool> Delete(int id);
    }
}
