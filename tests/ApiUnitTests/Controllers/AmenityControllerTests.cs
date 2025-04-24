using Api.Controllers;
using Application.DTOs;
using Application.Request.AmenityRequest;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiUnitTests.Controllers
{
    public class AmenityControllerTests
    {
        private readonly Mock<IAmenityService> _amenityServiceMock;
        private readonly AmenityController _controller;

        public AmenityControllerTests()
        {
            _amenityServiceMock = new Mock<IAmenityService>();
            _controller = new AmenityController(_amenityServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenAmenityExists()
        {
            // Arrange
            var amenityDto = new AmenityDTO { Id = 1, ServiceName = "Haircut" };
            _amenityServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(amenityDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(amenityDto);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithAmenitiesList()
        {
            // Arrange
            var amenities = new List<AmenityDTO>
            {
                new AmenityDTO { Id = 1, ServiceName = "Haircut" },
                new AmenityDTO { Id = 2, ServiceName = "Beard Trim" }
            };
            _amenityServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(amenities);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(amenities);
            _amenityServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithAmenityId()
        {
            // Arrange
            var request = new CreateAmenityRequest
            {
                ServiceName = "New Service",
                Description = "",

            };
            var amenityId = 1;
            _amenityServiceMock.Setup(x => x.Add(It.IsAny<CreateAmenityRequest>()))
                .ReturnsAsync(amenityId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result as CreatedAtActionResult;
            createdAtResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdAtResult.Value.Should().BeEquivalentTo(new { Id = amenityId });
            createdAtResult.RouteValues["id"].Should().Be(amenityId);
            _amenityServiceMock.Verify(x => x.Add(request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var request = new UpdateAmenityRequest
            {
                Description = "",
                ServiceName = "Update service"
            };
            _amenityServiceMock.Setup(x => x.Update(It.IsAny<UpdateAmenityRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _amenityServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            _amenityServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _amenityServiceMock.Verify(x => x.Delete(1), Times.Once);
        }
    }
}
