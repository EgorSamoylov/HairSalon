using Application.DTOs;
using Application.Request.AppointmentRequest;

namespace Application.Services
{
    public interface IAppointmentService
    {
        public Task<AppointmentDTO?> GetById(int id);
        public Task<IEnumerable<AppointmentDTO>> GetAll();
        public Task Add(CreateAppointmentRequest appointment);
        public Task<bool> Update(UpdateAppointmentRequest appointment);
        public Task<bool> Delete(int id);
    }
}
