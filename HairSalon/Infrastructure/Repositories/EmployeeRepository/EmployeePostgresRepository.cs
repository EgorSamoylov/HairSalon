using Domain.Entities;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.EmployeeRepository
{
    public class EmployeePostgresRepository : IEmployeeRepository
    {
        private readonly NpgsqlConnection _connection;

        public EmployeePostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Task Create(Employee employee)
        {
            await _connection.OpenAsync();

            var employeeId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  employees (first_name, last_name, phone_number, email, position)
                VALUES (@FirstName, @LastName, @PhoneNumber, @email, @Position)
                RETURNING id",
                new { employee.FirstName, employee.LastName, employee.PhoneNumber, employee.email, employee.Position });

            await _connection.CloseAsync();

            return employeeId;
        }

        public Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectRows = await _connection.ExecuteAsync(
                @"DELETE FROM employees WHERE @Id = id",
                new { Id = id });

            await _connection.CloseAsync();

            return affectRows > 0;
        }

        public Task<List<Employee>> ReadAll()
        {
            await _connection.OpenAsync();

            var employees = await _connection.QueryAsync<Employee>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email, 
                    position
                FROM employees");

            await _connection.CloseAsync();

            return employees.ToList();
        }

        public Task<Employee?> ReadById(int id)
        {
            await _connection.OpenAsync();

            var employee = await _connection.QueryFirstOrDefaultAsync<Employee>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email, 
                    position
                FROM employees
                WHERE Id = @id", new { Id = id });

            await _connection.CloseAsync();

            return employee;
        }

        public Task<bool> Update(Employee employee)
        {
            await _connection.OpenAsync();

            var AffectedRows = await _connection.ExecuteAsync(
                @"UPDATE employees
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        position = @Position
                    WHERE Id = @id",
                employee);

            await _connection.CloseAsync();

            return AffectedRows > 0;
        }
    }
}
