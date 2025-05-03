using Application.Mappings;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAmenityService, AmenityService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBCryptHasher, BCryptHasher>();
            services.AddTransient<IAttachmentService, AttachmentService>();
            services.AddTransient<IFileStorageService, FileStorageService>();

            // для работы валидаторов
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
