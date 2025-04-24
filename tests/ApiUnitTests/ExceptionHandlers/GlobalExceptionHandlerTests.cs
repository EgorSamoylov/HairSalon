using Api.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;

namespace ApiUnitTests.ExceptionHandlers
{
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
        private readonly GlobalExceptionHandler _handler;
        private readonly DefaultHttpContext _httpContext;

        public GlobalExceptionHandlerTests()
        {
            _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            _handler = new GlobalExceptionHandler(_problemDetailsServiceMock.Object);
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task TryHandleAsync_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldSetInternalServerErrorStatusCode()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldSetProblemJsonContentType()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
        }

        [Fact]
        public async Task TryHandleAsync_ShouldIncludeExceptionDetails()
        {
            // Arrange
            var exception = new Exception("Test error with details");
            _httpContext.Request.Path = "/api/details";

            // Act
            await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            // Assert
            _problemDetailsServiceMock.Verify(x => x.WriteAsync(It.Is<ProblemDetailsContext>(context =>
                context.ProblemDetails.Detail == exception.Message &&
                context.ProblemDetails.Type == exception.GetType().Name
            )));
        }
    }
}
