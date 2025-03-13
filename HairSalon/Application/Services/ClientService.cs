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
        private readonly IClientRepository _clientRepository;
        private IMapper _mapper;

        public  ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<int> Add(ClientDTO client)
        {
            var mappedClient = _mapper.Map<Client>(client);
            if (mappedClient != null)
            {
                await _clientRepository.Create(mappedClient);
                return mappedClient.Id;
            }

            throw new ArgumentException("Failed to map ClientDTO to Client"); //Или return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _clientRepository.Delete(id);
        }

        public async Task<IEnumerable<ClientDTO>> GetAll()
        {
            var clients = await _clientRepository.ReadAll();
            var mappedClient = clients.Select(q => _mapper.Map<ClientDTO>(q)).ToList();
            return mappedClient;
        }

        public async Task<ClientDTO?> GetById(int id)
        {
            var client = await _clientRepository.ReadById(id);
            var mappedClient = _mapper.Map<ClientDTO>(client);
            return mappedClient;
        }

        public async Task<bool> Update(ClientDTO client)
        {
            var mappedClient = _mapper.Map<Client>(client);
            return await _clientRepository.Update(mappedClient);
        }
    }
}
