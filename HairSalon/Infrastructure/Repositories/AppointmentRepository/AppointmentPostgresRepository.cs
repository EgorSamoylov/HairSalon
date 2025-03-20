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

        public Task Create(Appointment appointment)
        {
            await _connection.OpenAsync();

            var appointmentId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  appointments (client_id, empployee_id, amenity_id, appointment_datetime, notes)
                VALUES (@ClientId, @EmployeeId, @AmenityId, @AppointmentDatetime, @Notes)
                RETURNING id",
                new { appointment.ClientId, appointment.EmployeeId, appointment.AmenityId, appointment.AppointmentDatetime, appointment.Notes });

            await _connection.CloseAsync();

            return appointmentId;
        }

        public Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectRows = await _connection.ExecuteAsync(
                @"DELETE FROM appointments WHERE @Id = id",
                new { Id = id });

            await _connection.CloseAsync();

            return affectRows > 0;
        }

        public Task<List<Appointment>> ReadAll()
        {
            await _connection.OpenAsync();

            var appointments = await _connection.QueryAsync<Appointment>(
                @"SELECT 
                    id, 
                    client_id, 
                    employee_id,
                    amenity_id,
                    appointment_datetime,
                    notes
                FROM appointments");

            await _connection.CloseAsync();

            return appointments.ToList();
        }

        public Task<Appointment?> ReadById(int id)
        {
            await _connection.OpenAsync();

            var appointment = await _connection.QueryFirstOrDefaultAsync<Appointment>(
                @"SELECT 
                    id, 
                    client_id, 
                    employee_id,
                    amenity_id,
                    appointment_datetime,
                    notes
                FROM appointments
                WHERE Id = @id", new { Id = id });

            await _connection.CloseAsync();

            return appointment;
        }

        public Task<bool> Update(Appointment appointment)
        {
            await _connection.OpenAsync();

            var AffectedRows = await _connection.ExecuteAsync(
                @"UPDATE appointments
                    SET client_id = @ClientId,
                        employee_id = @EmployeeId,
                        amenity_id = @AmenityId,
                        appointment_datetime = @AppointmentDatetime,
                        notes = @Notes
                    WHERE Id = @id",
                appointment);

            await _connection.CloseAsync();

            return AffectedRows > 0;
        }
    }
}
