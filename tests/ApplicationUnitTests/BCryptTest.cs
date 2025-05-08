using Application.Services;
using FluentAssertions;
using Moq;

namespace ApplicationUnitTests
{
    public class BCryptTest
    {
        private readonly Mock<IBCryptHasher> _mockHasher = new();

        [Fact]
        public void NewHashTest()
        {
            // Arrange
            var password = "password";
            var hash = "hashed_password";

            _mockHasher.Setup(x => x.HashPassword(password)).Returns(hash);
            _mockHasher.Setup(x => x.VerifyPassword(password, hash)).Returns(true);

            // Act
            var actualHash = _mockHasher.Object.HashPassword(password);
            var verifyResult = _mockHasher.Object.VerifyPassword(password, actualHash);

            // Assert
            verifyResult.Should().BeTrue();
        }
    }
}
