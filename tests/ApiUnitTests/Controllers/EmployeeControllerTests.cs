using Application.Request.EmployeeRequest;
using Application.Services;
using Application.DTOs;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using Api.Controllers;
using Bogus;
using Bogus.DataSets;

namespace ApiUnitTests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenEmployeeExists()
        {
            Faker faker = new Faker();
            // Arrange
            var employeeDto = new EmployeeDTO
            {
                Id = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Position = "Hairdresser",
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(employeeDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(employeeDto);
            _employeeServiceMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithEmployeesList()
        {
            Faker faker = new Faker();
            // Arrange
            var employees = new List<EmployeeDTO>
            {
                new EmployeeDTO 
                {
                    Id = 1,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Position = "Hairdresser",
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                },
                new EmployeeDTO 
                { 
                    Id = 2,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Position = "Hairdresser",
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                }
            };
            _employeeServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(employees);
            _employeeServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithEmployeeId()
        {
            Faker faker = new Faker();
            // Arrange
            var request = new CreateEmployeeRequest
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Position = "Hairdresser",
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            var employeeId = 1;
            _employeeServiceMock.Setup(x => x.Add(It.IsAny<CreateEmployeeRequest>()))
                .ReturnsAsync(employeeId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result as CreatedAtActionResult;
            createdAtResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdAtResult.Value.Should().BeEquivalentTo(new { Id = employeeId });
            createdAtResult.RouteValues["id"].Should().Be(employeeId);
            _employeeServiceMock.Verify(x => x.Add(request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            Faker faker = new Faker();
            // Arrange
            var request = new UpdateEmployeeRequest
            {
                EmployeeId = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Position = "Hairdresser",
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _employeeServiceMock.Setup(x => x.Update(It.IsAny<UpdateEmployeeRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _employeeServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            _employeeServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _employeeServiceMock.Verify(x => x.Delete(1), Times.Once);
        }
    }
}
