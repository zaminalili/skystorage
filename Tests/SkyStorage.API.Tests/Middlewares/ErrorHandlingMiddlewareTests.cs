using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Exceptions;
using System.Net;
using Xunit;

namespace SkyStorage.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> loggerMock;
    private readonly ErrorHandlingMiddleware middleware;

    public ErrorHandlingMiddlewareTests()
    {
        loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        middleware = new ErrorHandlingMiddleware(loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_NotFoundException_Returns404()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate next = (ctx) => throw new NotFoundException(nameof(User), "1");

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task InvokeAsync_MismatchException_Returns403()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate next = (ctx) => throw new MismatchException("User", "Current user");

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.Should().Be(403);

    }

    [Fact]
    public async Task InvokeAsync_FileNotFoundException_Returns400()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate next = (ctx) => throw new FileNotFoundException("File not found");

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task InvokeAsync_GenericException_Returns500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate next = (ctx) => throw new Exception("Something went wrong");

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        context.Response.StatusCode.Should().Be(500);
    }
}