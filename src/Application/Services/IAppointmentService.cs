using Application.DTOs;
using Application.Request.AmenityRequest;
using Application.Request.AppointmentRequest;

namespace Application.Services
{
    public interface IAppointmentService
    {
        public Task<AppointmentDTO?> GetById(int id);
        public Task<IEnumerable<AppointmentDTO>> GetAll();
        public Task<int> Add(CreateAppointmentRequest appointment);
        public Task Update(UpdateAppointmentRequest appointment);
        public Task Delete(int id);
    }
}
