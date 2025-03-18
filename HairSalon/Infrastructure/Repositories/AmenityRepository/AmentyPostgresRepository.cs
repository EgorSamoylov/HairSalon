using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Npgsql;
using Dapper;

namespace Infrastructure.Repositories.AmenityRepository
{
    public class AmentyPostgresRepository : IAmenityRepository
    {
        private readonly NpgsqlConnection _connection;

        // ToDo написать для своих классов

        public AmentyPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Task<Amenity?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Amenity>> ReadAll()
        {
            throw new NotImplementedException();
        }

        Task IAmenityRepository.Create(Amenity service)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Amenity service)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
