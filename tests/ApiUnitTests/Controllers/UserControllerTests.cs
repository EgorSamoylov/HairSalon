using Application.Request.ClientRequest;
using Application.Services;
using Application.DTOs;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using Api.Controllers;
using Bogus;
using Bogus.DataSets;


namespace ApiUnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;
        private readonly Faker faker;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
            faker = new Faker();
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenUserExists()
        {
            // Arrange
            var userDto = new UserDTO 
            { 
                Id = 1, 
                FirstName = faker.Person.FirstName, 
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _userServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(userDto);
            _userServiceMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithUsersList()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                },
                new UserDTO
                {
                    Id = 2,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                }
            };
            _userServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(users);
            _userServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithUserId()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            var userId = 1;
            _userServiceMock.Setup(x => x.Add(It.IsAny<CreateUserRequest>()))
                .ReturnsAsync(userId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result as CreatedAtActionResult;
            createdAtResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdAtResult.Value.Should().BeEquivalentTo(new { Id = userId });
            createdAtResult.RouteValues["id"].Should().Be(userId);
            _userServiceMock.Verify(x => x.Add(request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                UserId = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _userServiceMock.Setup(x => x.Update(It.IsAny<UpdateUserRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _userServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            _userServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _userServiceMock.Verify(x => x.Delete(1), Times.Once);
        }
    }
}
