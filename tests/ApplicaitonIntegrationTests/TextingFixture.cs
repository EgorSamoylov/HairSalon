using Application;
using Bogus;
using Domain.Entities;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Infrastructure;
using Infrastructure.Repositories.ClientRepository;
using Infrastructure.Repositories.EmployeeRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using System.Reflection;
using MigrationRunner = Infrastructure.Database.MigrationRunner;

namespace ApplicaitonIntegrationTests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class TestingFixture : IAsyncLifetime
    {
        private readonly Faker _faker;

        private Respawner _respawner = null!;

        public TestingFixture()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => { config.AddJsonFile("appsettings.json"); })
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructure();
                    services.AddApplication();
                    var connectionString = context.Configuration.GetConnectionString("PostgresDBIntegration");
                    if (string.IsNullOrWhiteSpace(connectionString))
                        throw new ApplicationException("PostgresDBIntegration connection string is empty");

                    services.AddSingleton(_ => new NpgsqlDataSourceBuilder(connectionString).Build());

                    services.AddTransient(sp =>
                    {
                        var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
                        return dataSource.CreateConnection();
                    });

                    services
                        .AddFluentMigratorCore()
                        .ConfigureRunner(rb => rb
                            .AddPostgres()
                            .WithGlobalConnectionString(connectionString)
                            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                        .Configure<SelectingProcessorAccessorOptions>(options => { options.ProcessorId = "Postgres"; });
                })
                .Build();

            ServiceProvider = host.Services;

            _faker = new Faker();
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task InitializeAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();

            var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
            migrationRunner.Run();

            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = ["VersionInfo"]
            });
        }
        public async Task<Client> CreateClient()
        {
            using var scope = ServiceProvider.CreateScope();
            var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var clientId = await clientRepository.Create(new Client
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone
            });

            var client = await clientRepository.ReadById(clientId);

            if (client == null)
                throw new Exception("Can't create client");

            return client;
        }
        public async Task<Employee> CreateEmployee()
        {
            using var scope = ServiceProvider.CreateScope();
            var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();

            var employeeId = await employeeRepository.Create(new Employee
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone,
                Position = "hairdresser"
            });

            var employee = await employeeRepository.ReadById(employeeId);

            if (employee == null)
                throw new Exception("Can't create employee");

            return employee;
        }
        public async Task DisposeAsync()
        {
            await ResetDatabase();
        }

        private async Task ResetDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();

            await _respawner.ResetAsync(connection);
        }
    }
}
