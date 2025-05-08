using Application.DTOs;
using Application.Request;
using Application.Request.ClientRequest;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Amenity, AmenityDTO>().ReverseMap();

            CreateMap<RegistrationRequest, User>();

            CreateMap<UpdateUserRequest, User>();
        }
    }
}
