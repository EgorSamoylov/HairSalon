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
        public Task<AppointmentDTO?> GetById(int id);
        public Task<IEnumerable<AppointmentDTO>> GetAll();
        public Task<int> Add(AppointmentDTO appointment);
        public Task<bool> Update(AppointmentDTO appointment);
        public Task<bool> Delete(int id);
    }
}
