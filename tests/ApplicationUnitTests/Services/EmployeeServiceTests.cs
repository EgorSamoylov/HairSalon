using Application.Exceptions;
using Application.Mappings;
using Application.Request.EmployeeRequest;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.EmployeeRepository;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly Employee _employee;
        private readonly Faker _faker;

        public EmployeeServiceTests()
        {
            _faker = new Faker();

            _employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                Position = "Hairdresser"
            };

            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Add_ShouldCreateEmployeeAndReturnId()
        {
            // Arrange
            var request = new CreateEmployeeRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                Position = "Stylist"
            };

            _employeeRepositoryMock.Setup(x => x.Create(It.IsAny<Employee>()))
                .ReturnsAsync(2);

            // Act
            var result = await _employeeService.Add(request);

            // Assert
            result.Should().Be(2);
            _employeeRepositoryMock.Verify(x => x.Create(It.Is<Employee>(e =>
                e.FirstName == request.FirstName &&
                e.LastName == request.LastName &&
                e.PhoneNumber == request.PhoneNumber &&
                e.Email == request.Email &&
                e.Position == request.Position
            )), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            _employeeRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _employeeService.Delete(1);

            // Assert
            _employeeRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Arrange
            _employeeRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityDeleteException>(() => _employeeService.Delete(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                _employee,
                new Employee
                {
                    Id = 2,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PhoneNumber = "5551234567",
                    Email = "alice@example.com",
                    Position = "Manager"
                }
            };

            _employeeRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == 1 && x.Position == "Hairdresser");
            result.Should().Contain(x => x.Id == 2 && x.Position == "Manager");
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee()
        {
            // Arrange
            _employeeRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync(_employee);

            // Act
            var result = await _employeeService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_employee.Id);
            result.Position.Should().Be(_employee.Position);
            _employeeRepositoryMock.Verify(x => x.ReadById(1), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Arrange
            _employeeRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() => _employeeService.GetById(1));
        }

        [Theory]
        [InlineData(1, "John", "Doe", "Hairdresser")]
        [InlineData(2, "Jane", "Smith", "Stylist")]
        [InlineData(3, "Alice", "Johnson", "Manager")]
        public async Task GetById_WithDifferentIds_ShouldReturnCorrectEmployee(int employeeId, string firstName, string lastName, string position)
        {
            // Arrange
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Email = _faker.Internet.Email(),
                Position = position
            };

            _employeeRepositoryMock.Setup(x => x.ReadById(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetById(employeeId);

            // Assert
            result.Id.Should().Be(employeeId);
            result.FirstName.Should().Be(firstName);
            result.Position.Should().Be(position);
            _employeeRepositoryMock.Verify(x => x.ReadById(employeeId), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var request = new UpdateEmployeeRequest
            {
                EmployeeId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Position = "Senior Hairdresser"
            };

            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>()))
                .ReturnsAsync(true);

            // Act
            await _employeeService.Update(request);

            // Assert
            _employeeRepositoryMock.Verify(x => x.Update(It.Is<Employee>(e =>
                e.Id == request.EmployeeId &&
                e.FirstName == request.FirstName &&
                e.LastName == request.LastName &&
                e.PhoneNumber == request.PhoneNumber &&
                e.Email == request.Email &&
                e.Position == request.Position
            )), Times.Once);
        }

        [Fact]
        public async Task Update_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateEmployeeRequest
            {
                EmployeeId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Position = "Senior Hairdresser"
            };

            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(() => _employeeService.Update(request));
        }
    }
}
