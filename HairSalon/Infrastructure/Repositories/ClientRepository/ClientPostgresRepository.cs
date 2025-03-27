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

        public async Task<int> Create(Client client)
        {
            var clientId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  clients (first_name, last_name, phone_number, email, note)
                VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Note)
                RETURNING id",
                new { client.FirstName, client.LastName, client.PhoneNumber, client.Email, client.Note });

            return clientId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM clients WHERE @Id = id",
                new { Id = id });

            return affectedRows > 0;
        }

        public async Task<List<Client>> ReadAll()
        {
            var clients = await _connection.QueryAsync<Client>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email,
                    note
                FROM clients");

            return clients.ToList();
        }

        public async Task<Client?> ReadById(int id)
        {
            var client = await _connection.QueryFirstOrDefaultAsync<Client>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email,
                    note
                FROM clients
                WHERE Id = @id", new { Id = id });

            return client;
        }

        public async Task<bool> Update(Client client)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE clients
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        note = @Note
                    WHERE Id = @id",
                client);

            return affectedRows > 0;
        }
    }
}
