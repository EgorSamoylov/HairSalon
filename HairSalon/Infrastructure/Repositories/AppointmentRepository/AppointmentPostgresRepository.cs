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

        public async Task<int> Create(Appointment appointment)
        {
            var appointmentId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO  appointments (client_id, employee_id, amenity_id, appointment_datetime, notes)
                VALUES (@ClientId, @EmployeeId, @AmenityId, @AppointmentDatetime, @Notes)
                RETURNING id",
                new { appointment.ClientId, appointment.EmployeeId, appointment.ServiceId, appointment.AppointmentDateTime, appointment.Notes });

            return appointmentId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM appointments WHERE @Id = id",
                new { Id = id });

            return affectedRows > 0;
        }

        public async Task<List<Appointment>> ReadAll()
        {
            var appointments = await _connection.QueryAsync<Appointment>(
                @"SELECT 
                    id, 
                    client_id, 
                    employee_id,
                    amenity_id,
                    appointment_datetime,
                    notes
                FROM appointments");

            return appointments.ToList();
        }

        public async Task<Appointment?> ReadById(int id)
        {
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

            return appointment;
        }

        public async Task<bool> Update(Appointment appointment)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE appointments
                    SET client_id = @ClientId,
                        employee_id = @EmployeeId,
                        amenity_id = @AmenityId,
                        appointment_datetime = @AppointmentDatetime,
                        notes = @Notes
                    WHERE Id = @id",
                appointment);

            return affectedRows > 0;
        }
    }
}
