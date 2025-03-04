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
    public class ServiceService : IServiceService
    {
        private IServiceRepository serviceRepository;
        private IMapper mapper;
        public ServiceService (IServiceRepository serviceRepository, IMapper mapper)
        {
            this.serviceRepository = serviceRepository;
            this.mapper = mapper;
        }
        public async Task Add(ServiceDTO service)
        {
            var mappedService = mapper.Map<Service>(service);
            if (mappedService != null)
            {
                await serviceRepository.Create(mappedService);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await serviceRepository.Delete(id);
        }

        public async Task<List<ServiceDTO>> GetAll()
        {
            var services = await serviceRepository.ReadAll();
            var mappedServices = services.Select(q => mapper.Map<ServiceDTO>(q)).ToList();
            return mappedServices;
        }

        public async Task<ServiceDTO?> GetById(int id)
        {
            var service = await serviceRepository.ReadById(id);
            var mappedService = mapper.Map<ServiceDTO>(service);
            return mappedService;
        }

        public async Task<bool> Update(ServiceDTO service)
        {
            var mappedService = mapper.Map<Service>(service);
            return await serviceRepository.Update(mappedService);
        }
    }
}
