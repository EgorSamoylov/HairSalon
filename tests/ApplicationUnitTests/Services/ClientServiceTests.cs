using Application.Exceptions;
using Application.Mappings;
using Application.Request.ClientRequest;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.ClientRepository;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly Client _client;
        private readonly Faker _faker;

        public ClientServiceTests()
        {
            _faker = new Faker();

            _client = new Client
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                Note = "Regular customer"
            };

            _clientRepositoryMock = new Mock<IClientRepository>();
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            var loggerMock = new Mock<ILogger<ClientService>>();
            _clientService = new ClientService(
                _clientRepositoryMock.Object, 
                _mapper,
                loggerMock.Object);
        }

        [Fact]
        public async Task Add_ShouldCreateClientAndReturnId()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                Note = "New customer"
            };

            _clientRepositoryMock.Setup(x => x.Create(It.IsAny<Client>()))
                .ReturnsAsync(2);

            // Act
            var result = await _clientService.Add(request);

            // Assert
            result.Should().Be(2);
            _clientRepositoryMock.Verify(x => x.Create(It.Is<Client>(c =>
                c.FirstName == request.FirstName &&
                c.LastName == request.LastName &&
                c.PhoneNumber == request.PhoneNumber &&
                c.Email == request.Email &&
                c.Note == request.Note
            )), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            _clientRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _clientService.Delete(1);

            // Assert
            _clientRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenClientNotFound_ShouldThrowException()
        {
            // Arrange
            _clientRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityDeleteException>(() => _clientService.Delete(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                _client,
                new Client
                {
                    Id = 2,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PhoneNumber = "5551234567",
                    Email = "alice@example.com",
                    Note = "VIP client"
                }
            };

            _clientRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(clients);

            // Act
            var result = await _clientService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == 1 && x.FirstName == "John");
            result.Should().Contain(x => x.Id == 2 && x.FirstName == "Alice");
        }

        [Fact]
        public async Task GetById_ShouldReturnClient()
        {
            // Arrange
            _clientRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync(_client);

            // Act
            var result = await _clientService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_client.Id);
            result.FirstName.Should().Be(_client.FirstName);
            _clientRepositoryMock.Verify(x => x.ReadById(1), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenClientNotFound_ShouldThrowException()
        {
            // Arrange
            _clientRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync((Client)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() => _clientService.GetById(1));
        }

        [Theory]
        [InlineData(1, "John", "Doe")]
        [InlineData(2, "Jane", "Smith")]
        [InlineData(3, "Alice", "Johnson")]
        public async Task GetById_WithDifferentIds_ShouldReturnCorrectClient(int clientId, string firstName, string lastName)
        {
            // Arrange
            var client = new Client
            {
                Id = clientId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Email = _faker.Internet.Email(),
                Note = $"Client note {clientId}"
            };

            _clientRepositoryMock.Setup(x => x.ReadById(clientId))
                .ReturnsAsync(client);

            // Act
            var result = await _clientService.GetById(clientId);

            // Assert
            result.Id.Should().Be(clientId);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
            _clientRepositoryMock.Verify(x => x.ReadById(clientId), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            _clientRepositoryMock.Setup(x => x.Update(It.IsAny<Client>()))
                .ReturnsAsync(true);

            // Act
            await _clientService.Update(request);

            // Assert
            _clientRepositoryMock.Verify(x => x.Update(It.Is<Client>(c =>
                c.Id == request.ClientId &&
                c.FirstName == request.FirstName &&
                c.LastName == request.LastName &&
                c.PhoneNumber == request.PhoneNumber &&
                c.Email == request.Email &&
                c.Note == request.Note
            )), Times.Once);
        }

        [Fact]
        public async Task Update_WhenClientNotFound_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            _clientRepositoryMock.Setup(x => x.Update(It.IsAny<Client>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(() => _clientService.Update(request));
        }
    }
}
