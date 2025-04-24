using Api.ExceptionHandlers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Data.Common;
namespace ApiUnitTests.ExceptionHandlers
{
    public class DbExceptionHandlerTests
    {
        private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
        private readonly DbExceptionHandler _handler;
        private readonly DefaultHttpContext _httpContext;

        public DbExceptionHandlerTests()
        {
            _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            _handler = new DbExceptionHandler(_problemDetailsServiceMock.Object);
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task TryHandleAsync_ShouldReturnTrue_ForDbException()
        {
            // Arrange
            var exception = new Mock<DbException>();
            exception.Setup(e => e.ErrorCode).Returns(500);

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception.Object, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(500, _httpContext.Response.StatusCode);
            Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldReturnFalse_ForNonDbException()
        {
            // Arrange
            var exception = new InvalidOperationException("Some error");

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
            _problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldHandleDifferentErrorCodes()
        {
            // Arrange
            var errorCodes = new[] { 400, 401, 404, 500, 503 };

            foreach (var errorCode in errorCodes)
            {
                var exception = new Mock<DbException>();
                exception.Setup(e => e.ErrorCode).Returns(errorCode);
                _httpContext.Response.StatusCode = StatusCodes.Status200OK; // Reset

                // Act
                var result = await _handler.TryHandleAsync(_httpContext, exception.Object, CancellationToken.None);

                // Assert
                Assert.True(result);
                Assert.Equal(errorCode, _httpContext.Response.StatusCode);
            }
        }

        [Fact]
        public async Task TryHandleAsync_ShouldNotModifyResponse_WhenReturningFalse()
        {
            // Arrange
            var originalStatusCode = StatusCodes.Status200OK;
            _httpContext.Response.StatusCode = originalStatusCode;
            var exception = new Exception("Generic error");

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _httpContext.Response.StatusCode.Should().Be(originalStatusCode);
            _httpContext.Response.ContentType.Should().BeNull();
        }
    }
}
