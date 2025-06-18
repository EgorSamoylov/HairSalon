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
                @"INSERT INTO appointments 
            (client_id, employee_id, service_id, appointment_datetime, notes, is_completed, is_cancelled)
        VALUES 
            (@ClientId, @EmployeeId, @ServiceId, @AppointmentDateTime, @Notes, @IsCompleted, @IsCancelled)
        RETURNING id",
                new
                {
                    appointment.ClientId,
                    appointment.EmployeeId,
                    appointment.ServiceId,
                    appointment.AppointmentDateTime,
                    appointment.Notes,
                    IsCompleted = false,
                    IsCancelled = false
                });

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
                    client_id AS clientId, 
                    employee_id AS employeeId,
                    service_id AS serviceId,
                    appointment_datetime AS appointmentDateTime,
                    notes
                FROM appointments");

            return appointments.ToList();
        }

        public async Task<Appointment?> ReadById(int id)
        {
            var appointment = await _connection.QueryFirstOrDefaultAsync<Appointment>(
                @"SELECT 
                    id, 
                    client_id AS clientId, 
                    employee_id AS employeeId,
                    service_id AS serviceId,
                    appointment_datetime AS appointmentDateTime,
                    notes
                FROM appointments
                WHERE Id = @id", new { Id = id });

            return appointment;
        }

        public async Task<bool> Update(Appointment appointment)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE appointments
        SET 
            client_id = @ClientId,
            employee_id = @EmployeeId,
            service_id = @ServiceId,
            appointment_datetime = @AppointmentDateTime,
            notes = @Notes,
            is_completed = @IsCompleted,
            is_cancelled = @IsCancelled
        WHERE Id = @Id",
                new
                {
                    appointment.Id,
                    appointment.ClientId,
                    appointment.EmployeeId,
                    appointment.ServiceId,
                    appointment.AppointmentDateTime,
                    appointment.Notes,
                    appointment.IsCompleted,
                    appointment.IsCancelled
                });

            return affectedRows > 0;
        }

        public async Task<List<Appointment>> GetByEmployee(int employeeId)
        {
            var appointments = await _connection.QueryAsync<Appointment>(
                @"SELECT 
            id, 
            client_id AS clientId, 
            employee_id AS employeeId,
            service_id AS serviceId,
            appointment_datetime AS appointmentDateTime,
            notes,
            is_completed AS isCompleted,
            is_cancelled AS isCancelled
        FROM appointments
        WHERE employee_id = @EmployeeId",
                new { EmployeeId = employeeId });

            return appointments.ToList();
        }

        public async Task<List<Appointment>> GetByClient(int clientId)
        {
            var appointments = await _connection.QueryAsync<Appointment>(
                @"SELECT 
            id, 
            client_id AS clientId, 
            employee_id AS employeeId,
            service_id AS serviceId,
            appointment_datetime AS appointmentDateTime,
            notes,
            is_completed AS isCompleted,
            is_cancelled AS isCancelled
        FROM appointments
        WHERE client_id = @ClientId",
                new { ClientId = clientId });

            return appointments.ToList();
        }
    }
}
