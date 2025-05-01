using Application.DTOs;
using Application.Request;
using Application.Request.ClientRequest;
using Application.Request.EmployeeRequest;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();

            CreateMap<Client, ClientDTO>().ReverseMap();

            CreateMap<Employee, EmployeeDTO>().ReverseMap();

            CreateMap<Amenity, AmenityDTO>().ReverseMap();

            CreateMap<RegistrationRequest, Client>();

            CreateMap<RegistrationRequest, Employee>();

            CreateMap<UpdateClientRequest, Client>();

            CreateMap<UpdateEmployeeRequest, Employee>();

        }
    }
}
