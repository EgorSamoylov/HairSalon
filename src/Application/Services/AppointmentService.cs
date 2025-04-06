using Application.DTOs;
using Application.Exceptions;
using Application.Request.AppointmentRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.AppointmentRepository;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateAppointmentRequest request)
        {
            var appointment = new Appointment()
            {
                ClientId = request.ClientId,
                EmployeeId = request.EmployeeId,
                ServiceId = request.AmenityId,
                AppointmentDateTime = request.AppointmentDateTime,
                Notes = request.Notes
            };

            return await _appointmentRepository.Create(appointment);
        }

        public async Task Delete(int id)
        {
            var result = await _appointmentRepository.Delete(id);
            if (!result)
            {
                throw new EntityDeleteException("Appointment for deletion not found");
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAll()
        {
            var appointments = await _appointmentRepository.ReadAll();
            var mappedAppointments = appointments.Select(q => _mapper.Map<AppointmentDTO>(q)).ToList();
            return mappedAppointments;
        }

        public async Task<AppointmentDTO?> GetById(int id)
        {
            var appointment = await _appointmentRepository.ReadById(id);
            if (appointment is null)
            {
                throw new NotFoundApplicationException("Appointment not found");
            }
            var mappedAppointment = _mapper.Map<AppointmentDTO>(appointment);
            return mappedAppointment;
        }

        public async Task Update(UpdateAppointmentRequest request)
        {
            var appointment = new Appointment()
            {
                Id = request.AppointmentId,
                ClientId = request.ClientId,
                EmployeeId = request.EmployeeId,
                ServiceId = request.AmenityId,
                AppointmentDateTime = request.AppointmentDateTime,
                Notes = request.Notes
            };
            var result = await _appointmentRepository.Update(appointment);
            if (!result)
            {
                throw new EntityUpdateException("Appointment wasn't updated");
            }
        }
    }
}
