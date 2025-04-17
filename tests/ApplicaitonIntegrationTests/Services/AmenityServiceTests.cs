using Application.Exceptions;
using Application.Request.AmenityRequest;
using Application.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicaitonIntegrationTests.Services
{
    public class AmenityServiceTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IAmenityService _amenityService;
        private readonly IEmployeeService _employeeService;

        public AmenityServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _amenityService = scope.ServiceProvider.GetRequiredService<IAmenityService>();
            _employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoAmenitiesExist()
        {
            // Arrange
            await _fixture.DisposeAsync();

            // Act
            var amenities = await _amenityService.GetAll();

            // Assert
            amenities.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldCreateAmenity_WithValidData()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            var request = new CreateAmenityRequest
            {
                ServiceName = "Haircut",
                Description = "Basic haircut service",
                AuthorId = employee.Id,
                Price = 500,
                DurationMinutes = 30
            };

            // Act
            var id = await _amenityService.Add(request);
            var createdAmenity = await _amenityService.GetById(id);

            // Assert
            createdAmenity.Should().NotBeNull();
            createdAmenity.ServiceName.Should().Be(request.ServiceName);
            createdAmenity.Description.Should().Be(request.Description);
            createdAmenity.Price.Should().Be(request.Price);
            createdAmenity.DurationMinutes.Should().Be(request.DurationMinutes);
        }

        [Fact]
        public async Task GetById_ShouldReturnAmenity_WhenAmenityExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            var request = new CreateAmenityRequest
            {
                ServiceName = "Coloring",
                Description = "Hair coloring service",
                AuthorId = employee.Id,
                Price = 1500,
                DurationMinutes = 90
            };
            var id = await _amenityService.Add(request);

            // Act
            var amenity = await _amenityService.GetById(id);

            // Assert
            amenity.Should().NotBeNull();
            amenity.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException_WhenAmenityNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _amenityService.GetById(nonExistingId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingAmenity()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            // Create initial amenity
            var createRequest = new CreateAmenityRequest
            {
                ServiceName = "Old Service",
                Description = "Old Description",
                AuthorId = employee.Id,
                Price = 100,
                DurationMinutes = 10
            };

            var id = await _amenityService.Add(createRequest);

            // Prepare update data
            var updateRequest = new UpdateAmenityRequest
            {
                AmenityId = id,
                ServiceName = "Updated Service",
                Description = "Updated Description",
                AuthorId = employee.Id,
                Price = 200,
                DurationMinutes = 20
            };

            // Act
            await _amenityService.Update(updateRequest);

            // Get updated amenity
            var updatedAmenity = await _amenityService.GetById(id);

            // Assert
            updatedAmenity.Should().NotBeNull();
            updatedAmenity.Id.Should().Be(id);
            updatedAmenity.ServiceName.Should().Be(updateRequest.ServiceName);
            updatedAmenity.Description.Should().Be(updateRequest.Description);
            updatedAmenity.Price.Should().Be(updateRequest.Price);
            updatedAmenity.DurationMinutes.Should().Be(updateRequest.DurationMinutes);
        }

        [Fact]
        public async Task Delete_ShouldRemoveAmenity()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            var request = new CreateAmenityRequest
            {
                ServiceName = "Temporary Service",
                Description = "To be deleted",
                AuthorId = employee.Id,
                Price = 100,
                DurationMinutes = 10
            };
            var id = await _amenityService.Add(request);

            // Act
            await _amenityService.Delete(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(
                () => _amenityService.GetById(id));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCreatedAmenities()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            var request1 = new CreateAmenityRequest
            {
                ServiceName = "Service 1",
                Description = "Description 1",
                AuthorId = employee.Id,
                Price = 100,
                DurationMinutes = 10
            };

            var request2 = new CreateAmenityRequest
            {
                ServiceName = "Service 2",
                Description = "Description 2",
                AuthorId = employee.Id,
                Price = 200,
                DurationMinutes = 20
            };

            await _amenityService.Add(request1);
            await _amenityService.Add(request2);

            // Act
            var amenities = (await _amenityService.GetAll()).ToList();

            // Assert
            amenities.Should().HaveCount(2);
            amenities.Should().Contain(a => a.ServiceName == request1.ServiceName);
            amenities.Should().Contain(a => a.ServiceName == request2.ServiceName);
        }

        [Fact]
        public async Task Update_ShouldThrowNotFoundException_WhenAmenityNotExists()
        {
            // Arrange
            await _fixture.DisposeAsync();
            var employee = await _fixture.CreateEmployee();

            var updateRequest = new UpdateAmenityRequest
            {
                AmenityId = 9999, // Non-existing ID
                ServiceName = "Non-existent",
                Description = "Should fail",
                AuthorId = employee.Id,
                Price = 100,
                DurationMinutes = 10
            };

            // Act & Assert
            await Assert.ThrowsAsync<EntityUpdateException>(
                () => _amenityService.Update(updateRequest));
        }
    }
}
