using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.AppointmentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private IMapper _mapper;

        public AppointmentService (IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(AppointmentDTO appointment)
        {
            var mappedAppointment = _mapper.Map<Appointment>(appointment);
            if (mappedAppointment != null)
            {
                await _appointmentRepository.Create(mappedAppointment);
                return mappedAppointment.Id;
            }

            throw new ArgumentException("Failed to map AppointmentDTO to Appointment"); //Или return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _appointmentRepository.Delete(id);
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
            var mappedAppointment = _mapper.Map<AppointmentDTO>(appointment);
            return mappedAppointment;
        }

        public async Task<bool> Update(AppointmentDTO appointment)
        {
            var mappedAppointment = _mapper.Map<Appointment>(appointment);
            return await _appointmentRepository.Update(mappedAppointment);
        }
    }
}
