using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ClientRepository
{
    public class ClientInMemoryRepository : IClientRepository
    {
        private List<Client> _clients = new List<Client>();

        public ClientInMemoryRepository() { }

        public Task Create(Client client)
        {
            _clients.Add(client);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!_clients.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            _clients.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Client>> ReadAll()
        {
            return Task.FromResult(_clients);
        }

        public Task<Client?> ReadById(int id)
        {
            var client = _clients.Find(x => x.Id == id);
            return Task.FromResult(client);
        }

        public Task<bool> Update(Client client)
        {
            var clientToUpdate = _clients.Find(x => x.Id == client.Id);

            if (clientToUpdate != null)
            {
                return Task.FromResult(false);
            }

            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.LastName = client.LastName;
            clientToUpdate.PhoneNumber = client.PhoneNumber;
            clientToUpdate.Email = client.Email;
            clientToUpdate.Note = client.Note;

            return Task.FromResult(true);
        }
    }
}
