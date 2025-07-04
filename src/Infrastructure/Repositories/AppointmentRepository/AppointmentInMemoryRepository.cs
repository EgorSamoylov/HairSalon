﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AppointmentRepository
{
    public class AppointmentInMemoryRepository : IAppointmentRepository
    {
        private List<Appointment> _appointments = new List<Appointment>();

        public AppointmentInMemoryRepository() { }

        public Task<int> Create(Appointment appointment)
        {
            _appointments.Add(appointment);
            return Task.FromResult(appointment.Id);
        }

        public Task<bool> Delete(int id)
        {
            if (!_appointments.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            _appointments.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Appointment>> GetByClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Appointment>> GetByEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Appointment>> ReadAll()
        {
            return Task.FromResult(_appointments);
        }

        public Task<Appointment?> ReadById(int id)
        {
            var appointment = _appointments.Find(x => x.Id == id);
            return Task.FromResult(appointment);
        }

        public Task<bool> Update(Appointment appointment)
        {
            var appointmentToUpdate = _appointments.Find(x => x.Id == appointment.Id);

            if (appointmentToUpdate != null)
            {
                return Task.FromResult(false);
            }

            appointmentToUpdate.ClientId = appointmentToUpdate.ClientId;
            appointmentToUpdate.EmployeeId = appointmentToUpdate.EmployeeId;
            appointmentToUpdate.ServiceId = appointmentToUpdate.ServiceId;
            appointmentToUpdate.AppointmentDateTime = appointmentToUpdate.AppointmentDateTime;
            appointmentToUpdate.Notes = appointmentToUpdate.Notes;

            return Task.FromResult(true);
        }
    }
}
