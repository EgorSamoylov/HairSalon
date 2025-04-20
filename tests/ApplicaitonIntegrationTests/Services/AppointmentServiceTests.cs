using Application.DTOs;
using Application.Exceptions;
using Application.Request.AmenityRequest;
using Application.Request.AppointmentRequest;
using Application.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicaitonIntegrationTests.Services
{
    public class AppointmentServiceTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IAppointmentService _appointmentService;
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private readonly IAmenityService _amenityService;

        public AppointmentServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
            _clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
            _employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
            _amenityService = scope.ServiceProvider.GetRequiredService<IAmenityService>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoAppointmentsExist()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Act
            var appointments = await _appointmentService.GetAll();

            // Assert
            appointments.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldCreateAppointment_WithValidData()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var client = await _fixture.CreateClient();
            var employee = await _fixture.CreateEmployee();
            var amenity = await CreateTestAmenity();

            var request = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "Test appointment"
            };

            // Act
            var id = await _appointmentService.Add(request);
            var createdAppointment = await _appointmentService.GetById(id);

            // Assert
            createdAppointment.Should().NotBeNull();
            createdAppointment.ClientId.Should().Be(request.ClientId);
            createdAppointment.EmployeeId.Should().Be(request.EmployeeId);
            createdAppointment.ServiceId.Should().Be(request.AmenityId); // Здесь проверяем ServiceId, а не AmenityId
            createdAppointment.AppointmentDateTime.Should().BeCloseTo(request.AppointmentDateTime, TimeSpan.FromSeconds(1));
            createdAppointment.Notes.Should().Be(request.Notes);
        }

        [Fact]
        public async Task GetById_ShouldReturnAppointment_WhenAppointmentExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var client = await _fixture.CreateClient();
            var employee = await _fixture.CreateEmployee();
            var amenity = await CreateTestAmenity();

            var request = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "Test appointment for GetById"
            };
            var id = await _appointmentService.Add(request);

            // Act
            var appointment = await _appointmentService.GetById(id);

            // Assert
            appointment.Should().NotBeNull();
            appointment.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException_WhenAppointmentNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _appointmentService.GetById(nonExistingId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingAppointment()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var client = await _fixture.CreateClient();
            var employee = await _fixture.CreateEmployee();
            var amenity = await CreateTestAmenity();

            // Create initial appointment
            var createRequest = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "Initial notes"
            };

            var id = await _appointmentService.Add(createRequest);

            // Prepare update data
            var updateRequest = new UpdateAppointmentRequest
            {
                AppointmentId = id,
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(2),
                Notes = "Updated notes"
            };

            // Act
            await _appointmentService.Update(updateRequest);

            // Get updated appointment
            var updatedAppointment = await _appointmentService.GetById(id);

            // Assert
            updatedAppointment.Should().NotBeNull();
            updatedAppointment.Id.Should().Be(id);
            updatedAppointment.ClientId.Should().Be(updateRequest.ClientId);
            updatedAppointment.EmployeeId.Should().Be(updateRequest.EmployeeId);
            updatedAppointment.ServiceId.Should().Be(updateRequest.AmenityId);
            updatedAppointment.AppointmentDateTime.Should().BeCloseTo(updateRequest.AppointmentDateTime, TimeSpan.FromSeconds(1));
            updatedAppointment.Notes.Should().Be(updateRequest.Notes);
        }

        [Fact]
        public async Task Delete_ShouldRemoveAppointment()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var client = await _fixture.CreateClient();
            var employee = await _fixture.CreateEmployee();
            var amenity = await CreateTestAmenity();

            var request = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "Appointment to delete"
            };
            var id = await _appointmentService.Add(request);

            // Act
            await _appointmentService.Delete(id);

            // Assert
            await FluentActions
                .Invoking(() => _appointmentService.GetById(id))
                .Should()
                .ThrowAsync<NotFoundApplicationException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCreatedAppointments()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var client = await _fixture.CreateClient();
            var employee = await _fixture.CreateEmployee();
            var amenity = await CreateTestAmenity();
            var request1 = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "First appointment"
            };

            var request2 = new CreateAppointmentRequest
            {
                ClientId = client.Id,
                EmployeeId = employee.Id,
                AmenityId = amenity.Id,
                AppointmentDateTime = DateTime.Now.AddDays(2),
                Notes = "Second appointment"
            };

            await _appointmentService.Add(request1);
            await _appointmentService.Add(request2);

            // Act
            var appointments = (await _appointmentService.GetAll()).ToList();

            // Assert
            appointments.Should().HaveCount(2);
            appointments.Should().Contain(a => a.Notes == request1.Notes);
            appointments.Should().Contain(a => a.Notes == request2.Notes);
        }
        private async Task<AmenityDTO> CreateTestAmenity()
        {
            var employee = await _fixture.CreateEmployee();
            var request = new CreateAmenityRequest
            {
                ServiceName = "Test Service",
                Description = "Test Description",
                AuthorId = employee.Id,
                Price = 100,
                DurationMinutes = 30
            };
            var id = await _amenityService.Add(request);
            return await _amenityService.GetById(id);
        }
    }
}
