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
        public Task<IEnumerable<Client>> ReadAll();
        public Task<int> Create(Client client);
        public Task<bool> Update(Client client);
        Task<Client?> ReadByEmail(string email);
        public Task<bool> Delete(int id);
    }
}
