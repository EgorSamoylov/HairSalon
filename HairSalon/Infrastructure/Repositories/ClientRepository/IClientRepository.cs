using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ClientRepository
{
    public interface IClientRepository
    {
        public Task<Client?> ReadById(int id);
        public Task<List<Client>> ReadAll();
        public Task Create(Client client);
        public Task<bool> Update(Client client);
        public Task<bool> Delete(int id);
    }
}
