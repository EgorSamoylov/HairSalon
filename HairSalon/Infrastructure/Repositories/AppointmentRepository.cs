using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private List<Appointment> appointments = new List<Appointment>();
        public AppointmentRepository() { }
        public Task Create(Appointment appointment)
        {
            appointments.Add(appointment);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!appointments.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            appointments.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Appointment>> ReadAll()
        {
            return Task.FromResult(appointments);
        }

        public Task<Appointment?> ReadById(int id)
        {
            var appointment = appointments.Find(x => x.Id == id);
            return Task.FromResult(appointment);
        }

        public Task<bool> Update(Appointment appointment)
        {
            var appointmentToUpdate = appointments.Find(x => x.Id == appointment.Id);

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
