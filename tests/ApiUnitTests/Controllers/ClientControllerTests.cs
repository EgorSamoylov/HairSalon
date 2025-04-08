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
    public class ClientControllerTests
    {
        private readonly Mock<IClientService> _clientServiceMock;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _controller = new ClientController(_clientServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenClientExists()
        {
            Faker faker = new Faker();
            // Arrange
            var clientDto = new ClientDTO 
            { 
                Id = 1, 
                FirstName = faker.Person.FirstName, 
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _clientServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(clientDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(clientDto);
            _clientServiceMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithClientsList()
        {
            Faker faker = new Faker();
            // Arrange
            var clients = new List<ClientDTO>
            {
                new ClientDTO
                {
                    Id = 1,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                },
                new ClientDTO
                {
                    Id = 2,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    PhoneNumber = faker.Person.Phone
                }
            };
            _clientServiceMock.Setup(x => x.GetAll())
                .ReturnsAsync(clients);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(clients);
            _clientServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithClientId()
        {
            Faker faker = new Faker();
            // Arrange
            var request = new CreateClientRequest
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            var clientId = 1;
            _clientServiceMock.Setup(x => x.Add(It.IsAny<CreateClientRequest>()))
                .ReturnsAsync(clientId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result as CreatedAtActionResult;
            createdAtResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdAtResult.Value.Should().BeEquivalentTo(new { Id = clientId });
            createdAtResult.RouteValues["id"].Should().Be(clientId);
            _clientServiceMock.Verify(x => x.Add(request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccess()
        {
            Faker faker = new Faker();
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientId = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone
            };
            _clientServiceMock.Setup(x => x.Update(It.IsAny<UpdateClientRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _clientServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            _clientServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _clientServiceMock.Verify(x => x.Delete(1), Times.Once);
        }
    }
}
