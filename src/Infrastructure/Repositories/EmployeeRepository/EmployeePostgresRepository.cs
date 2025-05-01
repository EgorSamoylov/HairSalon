using Domain.Entities;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Infrastructure.Database.TypeMappings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories.EmployeeRepository
{
    public class EmployeePostgresRepository : IEmployeeRepository
    {
        private readonly NpgsqlConnection _connection;

        public EmployeePostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Employee employee)
        {
            const string query =
                @"INSERT INTO  employees (first_name, last_name, phone_number, email, position, password_hash, role)
                VALUES (@FirstName, @LastName, @PhoneNumber, @email, @Position, @Passwordhash, @Role::user_role)
                RETURNING id";

            return await _connection.ExecuteScalarAsync<int>(query, employee.AsDapperParams());
        }

        public async Task<bool> Delete(int id)
        {
            const string query = "DELETE FROM employees WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<Employee?> ReadByEmail(string email)
        {
            const string query = "SELECT * FROM users WHERE email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<Employee>(query, new { Email = email });
        }


        public async Task<IEnumerable<Employee>> ReadAll()
        {
            const string query = "SELECT * FROM employees";
            return await _connection.QueryAsync<Employee>(query);
        }

        public async Task<Employee?> ReadById(int id)
        {
            const string query = "SELECT * FROM employees WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Employee>(query, new { Id = id });
        }

        public async Task<bool> Update(Employee employee)
        {
            const string query =
                @"UPDATE employees
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        position = @Position
                    WHERE Id = @id";

            var affectedRows = await _connection.ExecuteAsync(query, employee.AsDapperParams());
            return affectedRows > 0;
        }
    }
}
