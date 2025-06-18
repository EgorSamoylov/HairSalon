using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AppointmentRepository
{
    public interface IAppointmentRepository
    {
        public Task<Appointment?> ReadById(int id);
        public Task<List<Appointment>> ReadAll();
        public Task<int> Create(Appointment appointment);
        public Task<bool> Update(Appointment appointment);
        public Task<bool> Delete(int id);
        public Task<List<Appointment>> GetByEmployee(int employeeId);
        public Task<List<Appointment>> GetByClient(int clientId);
    }
}
