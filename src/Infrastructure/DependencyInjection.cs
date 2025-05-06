using Dapper;
using FluentMigrator.Runner;
using Infrastructure.Database.TypeMappings;
using Infrastructure.Repositories.AmenityRepository;
using Infrastructure.Repositories.AppointmentRepository;
using Infrastructure.Repositories.AttachmentRepository;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("PostgresDB");
                return new NpgsqlDataSourceBuilder(connectionString).Build();
            });

            services.AddScoped(sp => // Scoped, так как connection можем переиспользовать
            {
                var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
                return dataSource.CreateConnection();
            });

            services.AddTransient<IAppointmentRepository, AppointmentPostgresRepository>();
            services.AddTransient<IUserRepository, UserPostgresRepository>();
            services.AddTransient<IAmenityRepository, AmenityPostgresRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentPostgresRepository>();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddFluentMigratorCore()
                    .ConfigureRunner(
                    rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString("PostgresDB")
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddScoped<Database.MigrationRunner>(); // наш мигратор

            return services;
        }
    }
}
