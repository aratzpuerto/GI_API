using GI_API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GI_API.Tests.Middleware
{
    [Trait("Category", "Middleware")]
    public class GlobalExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldCallNext_WhenNoException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            var middleware = new GlobalExceptionHandlingMiddleware(next, logger.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal(200, context.Response.StatusCode); // default
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn500_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = (ctx) => throw new Exception("Test error");

            var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            var middleware = new GlobalExceptionHandlingMiddleware(next, logger.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(500, context.Response.StatusCode);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("Server error", body);
        }

        [Fact]
        public async Task InvokeAsync_ShouldSetContentTypeAndReturnProblemDetails_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = (ctx) => throw new Exception("Test error");

            var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            var middleware = new GlobalExceptionHandlingMiddleware(next, logger.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert content type
            Assert.Equal("application/json", context.Response.ContentType);

            // Assert body content
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("\"status\":500", body);
            Assert.Contains("\"type\":\"Server error\"", body);
            Assert.Contains("\"title\":\"Server error\"", body);
            Assert.Contains("\"detail\":\"An internal server error has occured.\"", body);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogException_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var exception = new Exception("Test error");
            RequestDelegate next = (ctx) => throw exception;

            var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
            var middleware = new GlobalExceptionHandlingMiddleware(next, logger.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert using helper
            logger.VerifyLog(LogLevel.Error, "Unhandled exception occurred while processing request", Times.Once());
            logger.VerifyLog(LogLevel.Error, "Server error", Times.AtLeastOnce());
        }
    }
}
