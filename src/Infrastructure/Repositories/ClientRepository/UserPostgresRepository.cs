using Dapper;
using Domain.Entities;
using Infrastructure.Database.TypeMappings;
using Npgsql;

namespace Infrastructure.Repositories.ClientRepository
{
    public class UserPostgresRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection;

        public UserPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(User user)
        {
            const string query =
                @"INSERT INTO users 
            (first_name, last_name, phone_number, email, note, position, password_hash, role)
        VALUES 
            (@FirstName, @LastName, @PhoneNumber, @Email, @Note, @Position, @PasswordHash, @Role::user_role)
        RETURNING id";

            return await _connection.ExecuteScalarAsync<int>(query, new
            {
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.Note,
                user.Position,
                user.PasswordHash,
                Role = user.Role.ToString()
            });
        }

        public async Task<bool> Delete(int id)
        {
            const string query = "DELETE FROM users WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<User?> ReadByEmail(string email)
        {
            const string query = "SELECT id, first_name, last_name, phone_number, email, note, position, role::text, logo_attachment_id, password_hash FROM users WHERE email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<IEnumerable<User>> ReadAll()
        {
            const string query = "SELECT id, first_name, last_name, phone_number, email, note, position, role::text, logo_attachment_id  FROM users";
            return await _connection.QueryAsync<User>(query);
        }

        public async Task<User?> ReadById(int id)
        {
            const string query = "SELECT id, first_name, last_name, phone_number, email, note, position, role::text, logo_attachment_id FROM users WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
        }

        public async Task<bool> Update(User user)
        {
            const string query =
                @"UPDATE users
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        note = @Note,
                        position = @Position
                    WHERE Id = @id";

            var affectedRows = await _connection.ExecuteAsync(query, user.AsDapperParams());
            return affectedRows > 0;
        }

        public async Task<string?> GetUserRoleById(int id)
        {
            const string query = "SELECT role::text FROM users WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<string>(query, new { Id = id });
        }
    }
}
