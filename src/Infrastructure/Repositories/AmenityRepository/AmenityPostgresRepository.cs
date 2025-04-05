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
    public class AmenityPostgresRepository : IAmenityRepository
    {
        private readonly NpgsqlConnection _connection;

        public AmenityPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Amenity?> ReadById(int id)
        {
            var amenity = await _connection.QueryFirstOrDefaultAsync<Amenity>(
                @"SELECT 
                    id, 
                    service_name AS serviceName,
                    description,
                    author_id AS authorId,
                    price,  
                    duration_minutes as durationMinutes
                FROM amenities
                WHERE Id = @id", new { Id = id });

            return amenity;
        }

        public async Task<List<Amenity>> ReadAll()
        {
            var amenities = await _connection.QueryAsync<Amenity>(
                @"SELECT
                    id, 
                    author_id AS authorId,
                    description,
                    duration_minutes durationMinutes,
                    price,
                    service_name serviceName
                FROM amenities");

            return amenities.ToList();
        }

        public async Task<int> Create(Amenity amenity)
        {
            var amenityId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  amenities (service_name, description, author_id, price, duration_minutes)
                VALUES (@ServiceName, @Description, @AuthorId, @Price, @DurationMinutes)
                RETURNING id",
                new { amenity.ServiceName, amenity.Description, amenity.AuthorId, amenity.Price, amenity.DurationMinutes });

            return amenityId;
        }

        public async Task<bool> Update(Amenity amenity)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE amenities
                    SET service_name = @ServiceName,
                        description = @Description,
                        author_Id = @AuthorId,
                        price = @Price,
                        duration_minutes = @DurationMinutes
                    WHERE Id = @id",
                amenity);

            return affectedRows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM amenities WHERE @Id = id",
                new { Id = id });

            return affectedRows > 0;
        }
    }
}
