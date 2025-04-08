using Application.Mappings;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IAmenityService, AmenityService>();

            // для работы валидаторов
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
