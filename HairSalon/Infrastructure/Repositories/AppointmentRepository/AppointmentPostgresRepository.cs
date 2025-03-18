using Domain.Entities;
using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AppointmentRepository
{
    public class AppointmentPostgresRepository : IAppointmentRepository
    {
        private readonly NpgsqlConnection _connection;

        public AppointmentPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        // ToDo написать для своих классов

        public Task Create(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Appointment>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<Appointment?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
