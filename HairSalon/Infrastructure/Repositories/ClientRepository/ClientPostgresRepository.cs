using Domain.Entities;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ClientRepository
{
    public class ClientPostgresRepository : IClientRepository
    {
        private readonly NpgsqlConnection _connection;

        public ClientPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        // ToDo написать для своих классов

        public Task Create(Client client)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<Client?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
