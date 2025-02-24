using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private List<Client> clients = new List<Client>();
        public ClientRepository() { }
        public Task Create(Client client)
        {
            clients.Add(client);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!clients.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            clients.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Client>> ReadAll()
        {
            return Task.FromResult(clients);
        }

        public Task<Client?> ReadById(int id)
        {
            var client = clients.Find(x => x.Id == id);
            return Task.FromResult(client);
        }

        public Task<bool> Update(Client client)
        {
            var clientToUpdate = clients.Find(x => x.Id == client.Id);

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
