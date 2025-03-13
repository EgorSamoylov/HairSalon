using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AmenityRepository : IAmenityRepository
    {
        private List<Amenity> _amenities = new List<Amenity>();

        public AmenityRepository() { }

        public Task Create(Amenity amenity)
        {
            _amenities.Add(amenity);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!_amenities.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            _amenities.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Amenity>> ReadAll()
        {
            return Task.FromResult(_amenities);
        }

        public Task<Amenity?> ReadById(int id)
        {
            var amenity = _amenities.Find(x => x.Id == id);
            return Task.FromResult(amenity);
        }

        public Task<bool> Update(Amenity amenity)
        {
            var serviceToUpdate = _amenities.Find(x => x.Id == amenity.Id);

            if (serviceToUpdate != null)
            {
                return Task.FromResult(false);
            }

            serviceToUpdate.ServiceName = amenity.ServiceName;
            serviceToUpdate.Description = amenity.Description;
            serviceToUpdate.AuthorId = amenity.AuthorId;
            serviceToUpdate.Price = amenity.Price;
            serviceToUpdate.DurationMinutes = amenity.DurationMinutes;

            return Task.FromResult(true);
        }
    }
}
