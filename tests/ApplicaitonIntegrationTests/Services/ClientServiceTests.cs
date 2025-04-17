using Application.Exceptions;
using Application.Request.ClientRequest;
using Application.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicaitonIntegrationTests.Services
{
    public class ClientServiceTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IClientService _clientService;

        public ClientServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoClientsExist()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Act
            var clients = await _clientService.GetAll();

            // Assert
            clients.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldCreateClient_WithValidData()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateClientRequest
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+1234567890",
                Email = "john.doe@example.com",
                Note = "Regular customer"
            };

            // Act
            var id = await _clientService.Add(request);
            var createdClient = await _clientService.GetById(id);

            // Assert
            createdClient.Should().NotBeNull();
            createdClient.Id.Should().Be(id);
            createdClient.FirstName.Should().Be(request.FirstName);
            createdClient.LastName.Should().Be(request.LastName);
            createdClient.PhoneNumber.Should().Be(request.PhoneNumber);
            createdClient.Email.Should().Be(request.Email);
            createdClient.Note.Should().Be(request.Note);
        }

        [Fact]
        public async Task GetById_ShouldReturnClient_WhenClientExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateClientRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "+0987654321",
                Email = "jane.smith@example.com"
            };
            var id = await _clientService.Add(request);

            // Act
            var client = await _clientService.GetById(id);

            // Assert
            client.Should().NotBeNull();
            client.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException_WhenClientNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _clientService.GetById(nonExistingId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingClient()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Create initial client
            var createRequest = new CreateClientRequest
            {
                FirstName = "Initial",
                LastName = "Client",
                PhoneNumber = "+1111111111",
                Email = "initial@example.com",
                Note = "Initial note"
            };

            var id = await _clientService.Add(createRequest);

            // Prepare update data
            var updateRequest = new UpdateClientRequest
            {
                ClientId = id,
                FirstName = "Updated",
                LastName = "Client",
                PhoneNumber = "+2222222222",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            // Act
            await _clientService.Update(updateRequest);

            // Get updated client
            var updatedClient = await _clientService.GetById(id);

            // Assert
            updatedClient.Should().NotBeNull();
            updatedClient.Id.Should().Be(id);
            updatedClient.FirstName.Should().Be(updateRequest.FirstName);
            updatedClient.LastName.Should().Be(updateRequest.LastName);
            updatedClient.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
            updatedClient.Email.Should().Be(updateRequest.Email);
            updatedClient.Note.Should().Be(updateRequest.Note);
        }

        [Fact]
        public async Task Delete_ShouldRemoveClient()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateClientRequest
            {
                FirstName = "ToDelete",
                LastName = "Client",
                PhoneNumber = "+3333333333",
                Email = "delete@example.com"
            };
            var id = await _clientService.Add(request);

            // Act
            await _clientService.Delete(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _clientService.GetById(id));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCreatedClients()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request1 = new CreateClientRequest
            {
                FirstName = "Client1",
                LastName = "One",
                PhoneNumber = "+1111111111",
                Email = "client1@example.com"
            };

            var request2 = new CreateClientRequest
            {
                FirstName = "Client2",
                LastName = "Two",
                PhoneNumber = "+2222222222",
                Email = "client2@example.com"
            };

            await _clientService.Add(request1);
            await _clientService.Add(request2);

            // Act
            var clients = (await _clientService.GetAll()).ToList();

            // Assert
            clients.Should().HaveCount(2);
            clients.Should().Contain(c => c.Email == request1.Email);
            clients.Should().Contain(c => c.Email == request2.Email);
        }
    }
}
