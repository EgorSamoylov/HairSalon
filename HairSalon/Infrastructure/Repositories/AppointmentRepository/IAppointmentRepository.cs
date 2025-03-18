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
        public Task Create(Appointment appointment);
        public Task<bool> Update(Appointment appointment);
        public Task<bool> Delete(int id);
    }
}
