using Application.DTOs;
using Application.Request.AppointmentRequest;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using Application.Exceptions;
using Api.Controllers;

namespace ApiUnitTests.Controllers
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _appointmentServiceMock;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _appointmentServiceMock = new Mock<IAppointmentService>();
            _controller = new AppointmentController(_appointmentServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenAppointmentExists()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO { Id = 1, ClientId = 1 };
            _appointmentServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(appointmentDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(appointmentDto);
            _appointmentServiceMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithAppointmentsList()
        {
            // Arrange
            var appointments = new List<AppointmentDTO>
            {
                new AppointmentDTO { Id = 1, ClientId = 1 },
                new AppointmentDTO { Id = 2, ClientId = 2 }
            };
            _appointmentServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(appointments);
            _appointmentServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithAppointmentId()
        {
            // Arrange
            var request = new CreateAppointmentRequest { ClientId = 1 };
            var appointmentId = 1;
            _appointmentServiceMock.Setup(x => x.Add(It.IsAny<CreateAppointmentRequest>()))
                .ReturnsAsync(appointmentId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result as CreatedAtActionResult;
            createdAtResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdAtResult.Value.Should().BeEquivalentTo(new { Id = appointmentId });
            createdAtResult.RouteValues["id"].Should().Be(appointmentId);
            _appointmentServiceMock.Verify(x => x.Add(request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var request = new UpdateAppointmentRequest { AppointmentId = 1 };
            _appointmentServiceMock.Setup(x => x.Update(It.IsAny<UpdateAppointmentRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _appointmentServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            _appointmentServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _appointmentServiceMock.Verify(x => x.Delete(1), Times.Once);
        }
    }
}
