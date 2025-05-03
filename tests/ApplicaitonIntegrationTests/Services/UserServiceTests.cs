using Application.Exceptions;
using Application.Request.ClientRequest;
using Application.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicaitonIntegrationTests.Services
{
    public class UserServiceTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IUserService _userService;

        public UserServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Act
            var users = await _userService.GetAll();

            // Assert
            users.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldCreateUser_WithValidData()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+1234567890",
                Email = "john.doe@example.com",
                Note = "Regular customer"
            };

            // Act
            var id = await _userService.Add(request);
            var createdUser = await _userService.GetById(id);

            // Assert
            createdUser.Should().NotBeNull();
            createdUser.Id.Should().Be(id);
            createdUser.FirstName.Should().Be(request.FirstName);
            createdUser.LastName.Should().Be(request.LastName);
            createdUser.PhoneNumber.Should().Be(request.PhoneNumber);
            createdUser.Email.Should().Be(request.Email);
            createdUser.Note.Should().Be(request.Note);
            createdUser.Position.Should().Be(request.Position);
        }

        [Fact]
        public async Task GetById_ShouldReturnClient_WhenUserExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateUserRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "+0987654321",
                Email = "jane.smith@example.com"
            };
            var id = await _userService.Add(request);

            // Act
            var user = await _userService.GetById(id);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException_WhenUserNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _userService.GetById(nonExistingId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingUser()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Create initial client
            var createRequest = new CreateUserRequest
            {
                FirstName = "Initial",
                LastName = "Client",
                PhoneNumber = "+1111111111",
                Email = "initial@example.com",
                Note = "Initial note"
            };

            var id = await _userService.Add(createRequest);

            // Prepare update data
            var updateRequest = new UpdateUserRequest
            {
                UserId = id,
                FirstName = "Updated",
                LastName = "Client",
                PhoneNumber = "+2222222222",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            // Act
            await _userService.Update(updateRequest);

            // Get updated client
            var updatedUser = await _userService.GetById(id);

            // Assert
            updatedUser.Should().NotBeNull();
            updatedUser.Id.Should().Be(id);
            updatedUser.FirstName.Should().Be(updateRequest.FirstName);
            updatedUser.LastName.Should().Be(updateRequest.LastName);
            updatedUser.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
            updatedUser.Email.Should().Be(updateRequest.Email);
            updatedUser.Note.Should().Be(updateRequest.Note);
            updatedUser.Position.Should().Be(updateRequest.Position);
        }

        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request = new CreateUserRequest
            {
                FirstName = "ToDelete",
                LastName = "Client",
                PhoneNumber = "+3333333333",
                Email = "delete@example.com"
            };
            var id = await _userService.Add(request);

            // Act
            await _userService.Delete(id);

            // Assert
            await FluentActions
                .Invoking(() => _userService.GetById(id))
                .Should()
                .ThrowAsync<NotFoundApplicationException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCreatedUsers()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var request1 = new CreateUserRequest
            {
                FirstName = "Client1",
                LastName = "One",
                PhoneNumber = "+1111111111",
                Email = "client1@example.com"
            };

            var request2 = new CreateUserRequest
            {
                FirstName = "Client2",
                LastName = "Two",
                PhoneNumber = "+2222222222",
                Email = "client2@example.com"
            };

            await _userService.Add(request1);
            await _userService.Add(request2);

            // Act
            var users = (await _userService.GetAll()).ToList();

            // Assert
            users.Should().HaveCount(2);
            users.Should().Contain(c => c.Email == request1.Email);
            users.Should().Contain(c => c.Email == request2.Email);
        }
    }
}
