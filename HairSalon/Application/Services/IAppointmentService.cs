using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAppointmentService
    {
        public Task<AppointmentDTO?> ReadById(int id);
        public Task<List<AppointmentDTO>> ReadAll();
        public Task Create(AppointmentDTO appointment);
        public Task<bool> Update(AppointmentDTO appointment);
        public Task<bool> Delete(int id);
    }
}
