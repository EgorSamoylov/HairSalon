using Application.DTOs;
using Application.Exceptions;
using Application.Request.AppointmentRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.AmenityRepository;
using Infrastructure.Repositories.AppointmentRepository;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private IMapper _mapper;
        private readonly ILogger<IAppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository, 
            IMapper mapper,
            ILogger<IAppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _logger = logger;
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

            int appointmentId = await _appointmentRepository.Create(appointment);
            _logger.LogInformation(
                @"Appointment created with id {Id} by client {ClientId}, 
                by employee {EmployeeId},
                by service {ServiceId},
                with Appointment dateTime {AppointmentDateTime},
                Notes {Notes}",
                appointmentId,
                appointment.ClientId,
                appointment.EmployeeId,
                appointment.ServiceId,
                appointment.AppointmentDateTime,
                appointment.Notes);
            return appointmentId;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete appointment with id {Id}", id);

            var result = await _appointmentRepository.Delete(id);
            if (!result)
            {
                _logger.LogWarning("Appointment with id {Id} not found for deletion", id);
                throw new EntityDeleteException("Appointment for deletion not found");
            }

            _logger.LogInformation("Appointment with id {Id} successfully deleted", id);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAll()
        {
            _logger.LogInformation("Getting all appointments");

            var appointments = await _appointmentRepository.ReadAll();
            var mappedAppointments = appointments.Select(q => _mapper.Map<AppointmentDTO>(q));

            _logger.LogInformation("Retrieved {Count} appointments", mappedAppointments.Count());
            return mappedAppointments;
        }

        public async Task<AppointmentDTO> GetById(int id)
        {
            _logger.LogInformation("Getting appointment by id {Id}", id);

            var appointment = await _appointmentRepository.ReadById(id);
            if (appointment is null)
            {
                _logger.LogWarning("Appointment with id {Id} not found", id);
                throw new NotFoundApplicationException("Appointment not found");
            }

            var mappedAppointment = _mapper.Map<AppointmentDTO>(appointment);

            _logger.LogInformation(
                @"Retrieved appointment with id {Id}: 
                ClientId {ClientId},
                EmployeeId {EmployeeId},
                ServiceId {ServiceId},
                AppointmentDateTime {AppointmentDateTime},
                Notes {Notes}",
                id,
                appointment.ClientId,
                appointment.EmployeeId,
                appointment.ServiceId,
                appointment.AppointmentDateTime,
                appointment.Notes);

            return mappedAppointment;
        }

        public async Task Update(UpdateAppointmentRequest request)
        {
            _logger.LogInformation(
                @"Attempting to update appointment with id {Id}: 
                ClientId {ClientId},
                EmployeeId {EmployeeId},
                ServiceId {ServiceId},
                AppointmentDateTime {AppointmentDateTime},
                Notes {Notes}",
                request.AppointmentId,
                request.ClientId,
                request.EmployeeId,
                request.AmenityId,
                request.AppointmentDateTime,
                request.Notes);

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
                _logger.LogWarning("Appointment with id {Id} not found for update", request.AppointmentId);
                throw new EntityUpdateException("Appointment wasn't updated");
            }

            _logger.LogInformation("Appointment with id {Id} successfully updated", request.AppointmentId);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetByEmployee(int employeeId)
        {
            var appointments = await _appointmentRepository.GetByEmployee(employeeId);
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetByClient(int clientId)
        {
            var appointments = await _appointmentRepository.GetByClient(clientId);
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }

        public async Task UpdateStatus(int id, UpdateAppointmentStatusRequest request)
        {
            var appointment = await _appointmentRepository.ReadById(id);
            if (appointment == null)
                throw new NotFoundApplicationException("Appointment not found");

            if (request.IsCompleted.HasValue)
                appointment.IsCompleted = request.IsCompleted.Value;

            if (request.IsCancelled.HasValue)
                appointment.IsCancelled = request.IsCancelled.Value;

            await _appointmentRepository.Update(appointment);
        }
    }
}
