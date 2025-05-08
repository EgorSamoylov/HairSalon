using Application.Services;
using FluentAssertions;
using Moq;

namespace ApplicationUnitTests
{
    public class BCryptTest
    {
        private readonly Mock<IBCryptHasher> _mockHasher = new();

        [Fact]
        public void HashPassword_WhenGivenValidPassword_ReturnsHashThatCanBeVerified()
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

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsNull()
        {
            // Arrange
            string? nullPassword = null;
            var hash = "some_valid_hash";

            _mockHasher
                .Setup(x => x.VerifyPassword(null, It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _mockHasher.Object.VerifyPassword(nullPassword, hash);

            // Assert
            result.Should().BeFalse();
            _mockHasher.Verify(x => x.VerifyPassword(null, hash), Times.Once);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsEmpty()
        {
            // Arrange
            var emptyPassword = string.Empty;
            var hash = "some_valid_hash";

            _mockHasher
                .Setup(x => x.VerifyPassword(string.Empty, It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _mockHasher.Object.VerifyPassword(emptyPassword, hash);

            // Assert
            result.Should().BeFalse();
            _mockHasher.Verify(x => x.VerifyPassword(string.Empty, hash), Times.Once);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenStoredHashIsNull()
        {
            // Arrange
            var password = "valid_password";
            string? nullHash = null;

            _mockHasher
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), null))
                .Returns(false);

            // Act
            var result = _mockHasher.Object.VerifyPassword(password, nullHash);

            // Assert
            result.Should().BeFalse();
            _mockHasher.Verify(x => x.VerifyPassword(password, null), Times.Once);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenStoredHashIsEmpty()
        {
            // Arrange
            var password = "valid_password";
            var emptyHash = string.Empty;

            _mockHasher
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), string.Empty))
                .Returns(false);

            // Act
            var result = _mockHasher.Object.VerifyPassword(password, emptyHash);

            // Assert
            result.Should().BeFalse();
            _mockHasher.Verify(x => x.VerifyPassword(password, string.Empty), Times.Once);
        }
    }
}
