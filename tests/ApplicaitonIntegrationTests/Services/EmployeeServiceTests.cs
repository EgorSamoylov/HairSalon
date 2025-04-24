using Application.Exceptions;
using Application.Request.EmployeeRequest;
using Application.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicaitonIntegrationTests.Services
{
    public class EmployeeServiceTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IEmployeeService _employeeService;

        public EmployeeServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Act
            var employees = await _employeeService.GetAll();

            // Assert
            employees.Count().Should().Be(0);
        }

        [Fact]
        public async Task Add_ShouldCreateEmployee_WithValidData()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+1234567890",
                Email = "john.doe@example.com",
                Position = "hairdresser"
            };

            // Act
            var id = await _employeeService.Add(request);
            var createdEmployee = await _employeeService.GetById(id);

            // Assert
            createdEmployee.Should().NotBeNull();
            createdEmployee.Id.Should().Be(id);
            createdEmployee.FirstName.Should().Be(request.FirstName);
            createdEmployee.LastName.Should().Be(request.LastName);
            createdEmployee.PhoneNumber.Should().Be(request.PhoneNumber);
            createdEmployee.Email.Should().Be(request.Email);
            createdEmployee.Position.Should().Be(request.Position);
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateEmployeeRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "+0987654321",
                Email = "jane.smith@example.com",
                Position = "stylist"
            };
            var id = await _employeeService.Add(request);

            // Act
            var employee = await _employeeService.GetById(id);

            // Assert
            employee.Should().NotBeNull();
            employee.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException_WhenEmployeeNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _employeeService.GetById(nonExistingId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingEmployee()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Create initial employee
            var createRequest = new CreateEmployeeRequest
            {
                FirstName = "Initial",
                LastName = "Employee",
                PhoneNumber = "+1111111111",
                Email = "initial@example.com",
                Position = "junior"
            };

            var id = await _employeeService.Add(createRequest);

            // Prepare update data
            var updateRequest = new UpdateEmployeeRequest
            {
                EmployeeId = id,
                FirstName = "Updated",
                LastName = "Employee",
                PhoneNumber = "+2222222222",
                Email = "updated@example.com",
                Position = "senior"
            };

            // Act
            await _employeeService.Update(updateRequest);

            // Get updated employee
            var updatedEmployee = await _employeeService.GetById(id);

            // Assert
            updatedEmployee.Should().NotBeNull();
            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.FirstName.Should().Be(updateRequest.FirstName);
            updatedEmployee.LastName.Should().Be(updateRequest.LastName);
            updatedEmployee.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
            updatedEmployee.Email.Should().Be(updateRequest.Email);
            updatedEmployee.Position.Should().Be(updateRequest.Position);
        }

        [Fact]
        public async Task Delete_ShouldRemoveEmployee()
        {
            // Arrange
            var request = new CreateEmployeeRequest
            {
                FirstName = "ToDelete",
                LastName = "Employee",
                PhoneNumber = "+3333333333",
                Email = "delete@example.com",
                Position = "temporary"
            };
            var id = await _employeeService.Add(request);

            // Act
            await _employeeService.Delete(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _employeeService.GetById(id));

            // Дополнительная проверка, что повторное удаление выбрасывает правильное исключение
            await Assert.ThrowsAsync<EntityDeleteException>(
                () => _employeeService.Delete(id));

        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCreatedEmployees()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request1 = new CreateEmployeeRequest
            {
                FirstName = "Employee1",
                LastName = "One",
                PhoneNumber = "+1111111111",
                Email = "employee1@example.com",
                Position = "hairdresser"
            };

            var request2 = new CreateEmployeeRequest
            {
                FirstName = "Employee2",
                LastName = "Two",
                PhoneNumber = "+2222222222",
                Email = "employee2@example.com",
                Position = "stylist"
            };

            await _employeeService.Add(request1);
            await _employeeService.Add(request2);

            // Act
            var employees = (await _employeeService.GetAll()).ToList();

            // Assert
            employees.Should().HaveCount(2);
            employees.Should().Contain(e => e.Email == request1.Email);
            employees.Should().Contain(e => e.Email == request2.Email);
        }
    }
}
