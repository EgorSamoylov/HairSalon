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

        public Task Create(Client client)
        {
            await _connection.OpenAsync();

            var clientId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  clients (first_name, last_name, phone_number, email, note)
                VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Note)
                RETURNING id",
                new { client.FirstName, client.LastName, client.PhoneNumber, client.Email, client.Note });

            await _connection.CloseAsync();

            return clientId;
        }

        public Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectRows = await _connection.ExecuteAsync(
                @"DELETE FROM clients WHERE @Id = id",
                new { Id = id });

            await _connection.CloseAsync();

            return affectRows > 0;
        }

        public Task<List<Client>> ReadAll()
        {
            await _connection.OpenAsync();

            var clients = await _connection.QueryAsync<Client>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email,
                    note
                FROM clients");

            await _connection.CloseAsync();

            return clients.ToList();
        }

        public Task<Client?> ReadById(int id)
        {
            await _connection.OpenAsync();

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

            await _connection.CloseAsync();

            return client;
        }

        public Task<bool> Update(Client client)
        {
            await _connection.OpenAsync();

            var AffectedRows = await _connection.ExecuteAsync(
                @"UPDATE clients
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        note = @Note
                    WHERE Id = @id",
                client);

            await _connection.CloseAsync();

            return AffectedRows > 0;
        }
    }
}
