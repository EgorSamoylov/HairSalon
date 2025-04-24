using Api.ExceptionHandlers;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel.DataAnnotations;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApiUnitTests.ExceptionHandlers
{
    public class ApplicationExceptionHandlerTests
    {
        private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
        private readonly ApplicationExceptionHandler _handler;
        private readonly DefaultHttpContext _httpContext;

        public ApplicationExceptionHandlerTests()
        {
            _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            _handler = new ApplicationExceptionHandler(_problemDetailsServiceMock.Object);
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task TryHandleAsync_ShouldReturnTrue_ForBaseApplicationException()
        {
            // Arrange
            var exception = new NotFoundApplicationException("Not found");

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);
            Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldReturnFalse_ForNonApplicationException()
        {
            // Arrange
            var exception = new InvalidOperationException("Some error");

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.Equal(StatusCodes.Status200OK, _httpContext.Response.StatusCode); // Default status
            _problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
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
            Assert.False(result);
            Assert.Equal(originalStatusCode, _httpContext.Response.StatusCode);
            Assert.Null(_httpContext.Response.ContentType);
        }
    }
}
