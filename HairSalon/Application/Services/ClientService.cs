using Application.DTOs;
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
        public  ClientService(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }
        public Task Create(ClientDTO client)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ClientDTO>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<ClientDTO?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClientDTO client)
        {
            throw new NotImplementedException();
        }
    }
}
