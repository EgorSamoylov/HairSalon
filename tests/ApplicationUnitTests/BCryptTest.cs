using FluentAssertions;

namespace ApplicationUnitTests
{
    public class BCryptTest
    {
        [Fact]
        public void HashTest()
        {
            // Arrange
            var password = "password";
            var hash = HashPassword(password);

            // Act
            var verifyResult = VerifyPassword(password, hash);

            // Assert
            verifyResult.Should().BeTrue();
        }
        private string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return hash;
        }

        private bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
