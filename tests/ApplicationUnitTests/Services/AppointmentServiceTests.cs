using Application.Exceptions;
using Application.Mappings;
using Application.Request.AppointmentRequest;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.AppointmentRepository;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly Appointment _appointment;
        private readonly Faker _faker;

        public AppointmentServiceTests()
        {
            _faker = new Faker();

            _appointment = new Appointment
            {
                Id = 1,
                ClientId = 1,
                EmployeeId = 1,
                ServiceId = 1,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = "Initial notes"
            };

            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            _appointmentRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _appointmentService.Delete(1);

            // Assert
            _appointmentRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenAppointmentNotFound_ShouldThrowException()
        {
            // Arrange
            _appointmentRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityDeleteException>(() => _appointmentService.Delete(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                _appointment,
                new Appointment
                {
                    Id = 2,
                    ClientId = 2,
                    EmployeeId = 2,
                    ServiceId = 2,
                    AppointmentDateTime = DateTime.Now.AddDays(2),
                    Notes = "Second appointment"
                }
            };

            _appointmentRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == 1 && x.ClientId == 1);
            result.Should().Contain(x => x.Id == 2 && x.ClientId == 2);
        }

        [Fact]
        public async Task GetById_ShouldReturnAppointment()
        {
            // Arrange
            _appointmentRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync(_appointment);

            // Act
            var result = await _appointmentService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_appointment.Id);
            result.ClientId.Should().Be(_appointment.ClientId);
            _appointmentRepositoryMock.Verify(x => x.ReadById(1), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenAppointmentNotFound_ShouldThrowException()
        {
            // Arrange
            _appointmentRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() => _appointmentService.GetById(1));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 3, 3)]
        public async Task GetById_WithDifferentIds_ShouldReturnCorrectAppointment(int appointmentId, int clientId, int employeeId)
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = appointmentId,
                ClientId = clientId,
                EmployeeId = employeeId,
                ServiceId = 1,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Notes = $"Notes for appointment {appointmentId}"
            };

            _appointmentRepositoryMock.Setup(x => x.ReadById(appointmentId))
                .ReturnsAsync(appointment);

            // Act
            var result = await _appointmentService.GetById(appointmentId);

            // Assert
            result.Id.Should().Be(appointmentId);
            result.ClientId.Should().Be(clientId);
            result.EmployeeId.Should().Be(employeeId);
            _appointmentRepositoryMock.Verify(x => x.ReadById(appointmentId), Times.Once);
        }

        [Fact]
        public async Task Update_WhenAppointmentNotFound_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateAppointmentRequest
            {
                AppointmentId = 1,
                ClientId = 2,
                EmployeeId = 2,
                AmenityId = 2,
                AppointmentDateTime = DateTime.Now.AddDays(2),
                Notes = "Updated notes"
            };

            _appointmentRepositoryMock.Setup(x => x.Update(It.IsAny<Appointment>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(() => _appointmentService.Update(request));
        }
    }
}
