using Application.Exceptions;
using Application.Mappings;
using Application.Request.AmenityRequest;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.AmenityRepository;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class AmenityServiceTests
    {
        private readonly Mock<IAmenityRepository> _amenityRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IAmenityService _amenityService;
        private readonly Amenity _amenity;
        private readonly Faker _faker;

        public AmenityServiceTests()
        {
            _faker = new Faker();
            var author = new Employee()
            {
                Id = 1,
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Position = "Hairdresser"
            };

            _amenity = new Amenity
            {
                Id = 1,
                AuthorId = author.Id,
                ServiceName = "Haircut",
                Description = "Basic haircut service",
                DurationMinutes = 30,
                Price = 500,
            };

            _amenityRepositoryMock = new Mock<IAmenityRepository>();
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            var loggerMock = new Mock<ILogger<AmenityService>>();
            _amenityService = new AmenityService(
                _amenityRepositoryMock.Object, 
                _mapper,
                loggerMock.Object);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            _amenityRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _amenityService.Delete(1);

            // Assert
            _amenityRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenAmenityNotFound_ShouldThrowException()
        {
            // Arrange
            _amenityRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityDeleteException>(() => _amenityService.Delete(1));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAmenities()
        {
            // Arrange
            var amenities = new List<Amenity>
            {
                _amenity,
                new Amenity
                {
                    Id = 2,
                    ServiceName = "Beard Trim",
                    Description = "Beard trimming service",
                    DurationMinutes = 15,
                    Price = 200,
                    AuthorId = 1
                }
            };

            _amenityRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(amenities);

            // Act
            var result = await _amenityService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == 1 && x.ServiceName == "Haircut");
            result.Should().Contain(x => x.Id == 2 && x.ServiceName == "Beard Trim");
        }

        [Fact]
        public async Task GetById_ShouldReturnAmenity()
        {
            // Arrange
            _amenityRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync(_amenity);

            // Act
            var result = await _amenityService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_amenity.Id);
            result.ServiceName.Should().Be(_amenity.ServiceName);
            _amenityRepositoryMock.Verify(x => x.ReadById(1), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenAmenityNotFound_ShouldThrowException()
        {
            // Arrange
            _amenityRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
                .ReturnsAsync((Amenity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() => _amenityService.GetById(1));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public async Task GetById_WithDifferentIds_ShouldReturnCorrectAmenity(int amenityId, int authorId)
        {
            // Arrange
            var amenity = new Amenity
            {
                Id = amenityId,
                AuthorId = authorId,
                ServiceName = $"Service {amenityId}",
                Description = "",
                DurationMinutes = _faker.Random.Number(),
                Price = _faker.Random.Number()
            };

            _amenityRepositoryMock.Setup(x => x.ReadById(amenityId))
                .ReturnsAsync(amenity);

            // Act
            var result = await _amenityService.GetById(amenityId);

            // Assert
            result.Id.Should().Be(amenityId);
            result.AuthorId.Should().Be(authorId);
            _amenityRepositoryMock.Verify(x => x.ReadById(amenityId), Times.Once);
        }

        [Fact]
        public async Task Update_WhenAmenityNotFound_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateAmenityRequest
            {
                AmenityId = 1,
                ServiceName = "Updated Service",
                Description = "Updated Description",
                AuthorId = 1,
                Price = 600,
                DurationMinutes = 40
            };

            _amenityRepositoryMock.Setup(x => x.Update(It.IsAny<Amenity>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(() => _amenityService.Update(request));
        }
    }
}
