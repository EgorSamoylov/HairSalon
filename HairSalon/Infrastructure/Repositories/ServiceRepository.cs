using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private List<Service> services = new List<Service>();
        public ServiceRepository() {
            
        }
        public Task Create(Service service)
        {
            services.Add(service);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!services.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            services.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Service>> ReadAll()
        {
            return Task.FromResult(services);
        }

        public Task<Service?> ReadById(int id)
        {
            var service = services.Find(x => x.Id == id);
            return Task.FromResult(service);
        }

        public Task<bool> Update(Service service)
        {
            var serviceToUpdate = services.Find(x => x.Id == service.Id);

            if (serviceToUpdate != null)
            {
                return Task.FromResult(false);
            }

            serviceToUpdate.ServiceName = service.ServiceName;
            serviceToUpdate.Description = service.Description;
            serviceToUpdate.AuthorId = service.AuthorId;
            serviceToUpdate.Price = service.Price;
            serviceToUpdate.DurationMinutes = service.DurationMinutes;

            return Task.FromResult(true);
        }
    }
}
