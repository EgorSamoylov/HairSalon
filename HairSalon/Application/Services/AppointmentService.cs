using Application.DTOs;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        public AppointmentService (IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }
        public Task Create(AppointmentDTO appointment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentDTO>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDTO?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(AppointmentDTO appointment)
        {
            throw new NotImplementedException();
        }
    }
}
