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

        // ToDo написать для своих классов

        public Task Create(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
