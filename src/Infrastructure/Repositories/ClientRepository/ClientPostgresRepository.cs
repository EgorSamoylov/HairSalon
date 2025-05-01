using Dapper;
using Domain.Entities;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Infrastructure.Database.TypeMappings;

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
            const string query =
                @"INSERT INTO  clients (first_name, last_name, phone_number, email, note, password_hash, role)
                VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Note, @Passwordhash, @Role::user_role)
                RETURNING id";

            return await _connection.ExecuteScalarAsync<int>(query, client.AsDapperParams());
        }

        public async Task<bool> Delete(int id)
        {
            const string query = "DELETE FROM clients WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<Client?> ReadByEmail(string email)
        {
            const string query = "SELECT * FROM users WHERE email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<Client>(query, new { Email = email });
        }

        public async Task<IEnumerable<Client>> ReadAll()
        {
            const string query = "SELECT * FROM clients";
            return await _connection.QueryAsync<Client>(query);
        }

        public async Task<Client?> ReadById(int id)
        {
            const string query = "SELECT * FROM clients WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Client>(query, new { Id = id });
        }

        public async Task<bool> Update(Client client)
        {
            const string query =
                @"UPDATE clients
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        note = @Note
                    WHERE Id = @id";

            var affectedRows = await _connection.ExecuteAsync(query, client.AsDapperParams());
            return affectedRows > 0;
        }
    }
}
