using Application.DTOs;
using AutoMapper;
using Domain.Entities;
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
        private IMapper mapper;
        public AppointmentService (IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            this.appointmentRepository = appointmentRepository;
            this.mapper = mapper;
        }
        public async Task Add(AppointmentDTO appointment)
        {
            var mappedAppointment = mapper.Map<Appointment>(appointment);
            if (mappedAppointment != null)
            {
                await appointmentRepository.Create(mappedAppointment);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await appointmentRepository.Delete(id);
        }

        public async Task<List<AppointmentDTO>> GetAll()
        {
            var appointments = await appointmentRepository.ReadAll();
            var mappedAppointments = appointments.Select(q => mapper.Map<AppointmentDTO>(q)).ToList();
            return mappedAppointments;
        }

        public async Task<AppointmentDTO?> GetById(int id)
        {
            var appointment = await appointmentRepository.ReadById(id);
            var mappedAppointment = mapper.Map<AppointmentDTO>(appointment);
            return mappedAppointment;
        }

        public async Task<bool> Update(AppointmentDTO appointment)
        {
            var mappedAppointment = mapper.Map<Appointment>(appointment);
            return await appointmentRepository.Update(mappedAppointment);
        }
    }
}
