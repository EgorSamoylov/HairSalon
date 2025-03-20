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

        public AmentyPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Amenity?> ReadById(int id)
        {
            await _connection.OpenAsync();

            var amenity = await _connection.QueryFirstOrDefaultAsync<Amenity>(
                @"SELECT 
                    id, 
                    service_name, 
                    description
                    author_id
                    price
                    duration_minutes
                FROM amenities
                WHERE Id = @id", new { Id = id });

            await _connection.CloseAsync();

            return amenity;
        }

        public async Task<List<Amenity>> ReadAll()
        {
            await _connection.OpenAsync();

            var amenities = await _connection.QueryAsync<Amenity>(
                @"SELECT
                    id, 
                    service_name, 
                    description
                    author_id
                    price
                    duration_minutes
                FROM amenities");

            await _connection.CloseAsync();

            return amenities.ToList();
        }

        public async Task<int> Create(Amenity amenity)
        {
            await _connection.OpenAsync();

            var amenityId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  amenities (service_name, description, author_id, price, duration_minutes)
                VALUES (@ServiceName, @Description, @AuthorId, @Price, @DurationMinutes)
                RETURNING id",
                new { amenity.ServiceName, amenity.Description, amenity.AuthorId, amenity.Price, amentiy.DurationMinutes });

            await _connection.CloseAsync();

            return amenityId;
        }

        public async Task<bool> Update(Amenity amenity)
        {
            await _connection.OpenAsync();

            var AffectedRows = await _connection.ExecuteAsync(
                @"UPDATE amenities
                    SET service_name = @ServiceName,
                        description = @Description,
                        author_Id = @AuthorId,
                        price = @Price,
                        duration_minutes = @DurationMinutes
                    WHERE Id = @id",
                amenity);

            await _connection.CloseAsync();

            return AffectedRows > 0;
        }

        public Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectRows = await _connection.ExecuteAsync(
                @"DELETE FROM amenities WHERE @Id = id",
                new { Id = id });

            await _connection.CloseAsync();

            return affectRows > 0;
        }
    }
}
