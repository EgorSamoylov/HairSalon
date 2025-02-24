using Application.DTOs;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceService : IServiceService
    {
        private IServiceRepository serviceRepository;
        public ServiceService (IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }
        public Task Add(ServiceDTO service)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ServiceDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceDTO?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ServiceDTO service)
        {
            throw new NotImplementedException();
        }
    }
}
