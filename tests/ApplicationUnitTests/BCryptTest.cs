using Application.Services;
using FluentAssertions;
using Moq;

namespace ApplicationUnitTests
{
    public class BCryptTest
    {
        private readonly BCryptHasher _hasher = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void VerifyPassword_NullOrEmptyStoredHash_ReturnsFalse(string storedHash)
        {
            // Arrange
            var password = "A1234567";

            // Act
            var result = _hasher.VerifyPassword(password, storedHash);

            // Assert
            result.Should().BeFalse();
        }
    }
}
