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

        public async Task<int> Create(Employee employee)
        {
            var employeeId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  employees (first_name, last_name, phone_number, email, position)
                VALUES (@FirstName, @LastName, @PhoneNumber, @email, @Position)
                RETURNING id",
                new { employee.FirstName, employee.LastName, employee.PhoneNumber, employee.Email, employee.Position });

            return employeeId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectRows = await _connection.ExecuteAsync(
                @"DELETE FROM employees WHERE @Id = id",
                new { Id = id });

            return affectRows > 0;
        }

        public async Task<List<Employee>> ReadAll()
        {
            var employees = await _connection.QueryAsync<Employee>(
                @"SELECT 
                    id, 
                    first_name, 
                    last_name,
                    phone_number,
                    email, 
                    position
                FROM employees");

            return employees.ToList();
        }

        public async Task<Employee?> ReadById(int id)
        {
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

            return employee;
        }

        public async Task<bool> Update(Employee employee)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE employees
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        phone_number = @PhoneNumber,
                        email = @Email,
                        position = @Position
                    WHERE Id = @id",
                employee);

            return affectedRows > 0;
        }
    }
}
