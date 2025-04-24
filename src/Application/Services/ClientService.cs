using Application.DTOs;
using Application.Exceptions;
using Application.Request.ClientRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.Extensions.Logging;


namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private IMapper _mapper;
        private readonly ILogger<ClientService> _logger;


        public ClientService(
            IClientRepository clientRepository,
            IMapper mapper,
            ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateClientRequest request)
        {
            var client = new Client()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note
            };

            int clientId = await _clientRepository.Create(client);
            _logger.LogInformation(
                @"Client created with id {Id} with FirstName {FirstName}, 
                LastName {LastName},
                with PhoneNumber {PhoneNumber},
                with Email {Email},
                Note {Note}",
                clientId,
                client.FirstName,
                client.LastName,
                client.PhoneNumber,
                client.Email,
                client.Note);
            return clientId;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete client with id {Id}", id);

            var result = await _clientRepository.Delete(id);
            if (!result)
            {
                _logger.LogWarning("Client with id {Id} not found for deletion", id);
                throw new EntityDeleteException("Client for deletion not found");
            }

            _logger.LogInformation("Client with id {Id} successfully deleted", id);
        }

        public async Task<IEnumerable<ClientDTO>> GetAll()
        {
            _logger.LogInformation("Getting all clients");

            var clients = await _clientRepository.ReadAll();
            var mappedClients = clients.Select(q => _mapper.Map<ClientDTO>(q));

            _logger.LogInformation("Retrieved {Count} clients", mappedClients.Count());
            return mappedClients;
        }

        public async Task<ClientDTO> GetById(int id)
        {
            _logger.LogInformation("Getting client by id {Id}", id);

            var client = await _clientRepository.ReadById(id);
            if (client is null)
            {
                _logger.LogWarning("Client with id {Id} not found", id);
                throw new NotFoundApplicationException("Client not found");
            }

            var mappedClient = _mapper.Map<ClientDTO>(client);

            _logger.LogInformation(
                @"Retrieved client with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Note {Note}",
                id,
                client.FirstName,
                client.LastName,
                client.PhoneNumber,
                client.Email,
                client.Note);

            return mappedClient;
        }

        public async Task Update(UpdateClientRequest request)
        {
            _logger.LogInformation(
                @"Attempting to update client with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Note {Note}",
                request.ClientId,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                request.Note);

            var client = new Client()
            {
                Id = request.ClientId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Note = request.Note
            };

            var result = await _clientRepository.Update(client);
            if (!result)
            {
                _logger.LogWarning("Client with id {Id} not found for update", request.ClientId);
                throw new EntityUpdateException("Client wasn't updated");
            }

            _logger.LogInformation("Client with id {Id} successfully updated", request.ClientId);
        }
    }
}
