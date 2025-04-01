using Application.DTOs;
using Application.Request.ClientRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task Add(CreateClientRequest request)
        {
            var client = new Client()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note
            };

            await _clientRepository.Create(client);
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

        public async Task<bool> Update(UpdateClientRequest request)
        {
            var client = new Client()
            {
                Id = request.ClientId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note
            };
            return await _clientRepository.Update(client);
        }
    }
}
