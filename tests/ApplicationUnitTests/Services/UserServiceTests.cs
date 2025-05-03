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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly User _user;
        private readonly Faker _faker;

        public UserServiceTests()
        {
            _faker = new Faker();

            _user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                Note = "Regular customer",
            };

            _userRepositoryMock = new Mock<IUserRepository>();
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            var loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(
                _userRepositoryMock.Object, 
                _mapper,
                loggerMock.Object);
        }

        [Fact]
        public async Task Add_ShouldCreateClientAndReturnId()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                Note = "New customer"
            };

            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
                .ReturnsAsync(2);

            // Act
            var result = await _userService.Add(request);

            // Assert
            result.Should().Be(2);
            _userRepositoryMock.Verify(x => x.Create(It.Is<User>(c =>
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
            _userRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _userService.Delete(1);

            // Assert
            _userRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenClientNotFound_ShouldThrowException()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityDeleteException>(() => _userService.Delete(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllClients()
        {
            // Arrange
            var clients = new List<User>
            {
                _user,
                new User
                {
                    Id = 2,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PhoneNumber = "5551234567",
                    Email = "alice@example.com",
                    Note = "VIP client"
                }
            };

            _userRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(clients);

            // Act
            var result = await _userService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == 1 && x.FirstName == "John");
            result.Should().Contain(x => x.Id == 2 && x.FirstName == "Alice");
        }

        [Fact]
        public async Task GetById_ShouldReturnClient()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync(_user);

            // Act
            var result = await _userService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_user.Id);
            result.FirstName.Should().Be(_user.FirstName);
            _userRepositoryMock.Verify(x => x.ReadById(1), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenClientNotFound_ShouldThrowException()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() => _userService.GetById(1));
        }

        [Theory]
        [InlineData(1, "John", "Doe")]
        [InlineData(2, "Jane", "Smith")]
        [InlineData(3, "Alice", "Johnson")]
        public async Task GetById_WithDifferentIds_ShouldReturnCorrectClient(int clientId, string firstName, string lastName)
        {
            // Arrange
            var client = new User
            {
                Id = clientId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Email = _faker.Internet.Email(),
                Note = $"Client note {clientId}"
            };

            _userRepositoryMock.Setup(x => x.ReadById(clientId))
                .ReturnsAsync(client);

            // Act
            var result = await _userService.GetById(clientId);

            // Assert
            result.Id.Should().Be(clientId);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
            _userRepositoryMock.Verify(x => x.ReadById(clientId), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                UserId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await _userService.Update(request);

            // Assert
            _userRepositoryMock.Verify(x => x.Update(It.Is<User>(c =>
                c.Id == request.UserId &&
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
            var request = new UpdateUserRequest
            {
                UserId = 1,
                FirstName = "John Updated",
                LastName = "Doe Updated",
                PhoneNumber = "1112223333",
                Email = "updated@example.com",
                Note = "Updated note"
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(() => _userService.Update(request));
        }
    }
}
