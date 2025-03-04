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
    public class ClientService : IClientService
    {
        private IClientRepository clientRepository;
        private IMapper mapper;
        public  ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            this.clientRepository = clientRepository;
            this.mapper = mapper;
        }
        public async Task Add(ClientDTO client)
        {
            var mappedClient = mapper.Map<Client>(client);
            if (mappedClient != null)
            {
                await clientRepository.Create(mappedClient);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await clientRepository.Delete(id);
        }

        public async Task<List<ClientDTO>> GetAll()
        {
            var clients = await clientRepository.ReadAll();
            var mappedClient = clients.Select(q => mapper.Map<ClientDTO>(q)).ToList();
            return mappedClient;
        }

        public async Task<ClientDTO?> GetById(int id)
        {
            var client = await clientRepository.ReadById(id);
            var mappedClient = mapper.Map<ClientDTO>(client);
            return mappedClient;
        }

        public async Task<bool> Update(ClientDTO client)
        {
            var mappedClient = mapper.Map<Client>(client);
            return await clientRepository.Update(mappedClient);
        }
    }
}
