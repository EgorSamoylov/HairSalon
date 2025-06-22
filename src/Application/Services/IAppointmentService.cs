using Application.DTOs;
using Application.Request.AmenityRequest;
using Application.Request.AppointmentRequest;
using Domain.Entities;

namespace Application.Services
{
    public interface IAppointmentService
    {
        public Task<AppointmentDTO> GetById(int id);
        public Task<IEnumerable<AppointmentDTO>> GetAll();
        public Task<int> Add(CreateAppointmentRequest appointment);
        public Task Update(UpdateAppointmentRequest appointment);
        public Task Delete(int id);
        public Task<IEnumerable<AppointmentDTO>> GetByEmployee(int employeeId);
        public Task<IEnumerable<AppointmentDTO>> GetByClient(int clientId);
        public Task<IEnumerable<AppointmentDTO>> GetByUser(UserContext userContext);
        public Task UpdateStatus(int id, UpdateAppointmentStatusRequest request, UserContext userContext);
        public Task UpdateStatus(int id, UpdateAppointmentStatusRequest request);
    }
}
