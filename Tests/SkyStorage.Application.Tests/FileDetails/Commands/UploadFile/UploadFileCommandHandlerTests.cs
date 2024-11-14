using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SkyStorage.Application.Tests.Extensions;
using SkyStorage.Application.Users;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;
using System.Security.Claims;
using Xunit;

namespace SkyStorage.Application.FileDetails.Commands.UploadFile.Tests;

public class UploadFileCommandHandlerTests
{
    private readonly Mock<IBlobStorageService> blobStorageServiceMock;
    private readonly Mock<IFileDetailRepository> fileDetailRepositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ILogger<UploadFileCommandHandler>> loggerMock;
    private readonly Mock<IUserContext> userContextMock;

    public UploadFileCommandHandlerTests()
    {
        this.blobStorageServiceMock = new Mock<IBlobStorageService>();
        this.fileDetailRepositoryMock = new Mock<IFileDetailRepository>();
        this.mapperMock = new Mock<IMapper>();
        this.loggerMock = new Mock<ILogger<UploadFileCommandHandler>>();
        this.userContextMock = new Mock<IUserContext>();
    }

    [Fact]
    public async Task Handle_ValidRequest_UploadsFileAndSavesDetails()
    {
        // Arrange
        var request = new UploadFileCommand 
        {   
            userId = Guid.Parse("7927DE88-792A-409D-B6A4-E9E677672CC9"), 
            FileName = "testfile.txt", 
            ContentType = "text/plain",
            Size = 10,
            File = new MemoryStream(new byte[] { 0x1, 0x2 }) 
        };
        var fileDetail = new FileDetail 
        {   
            Id = Guid.NewGuid(), 
            FileName = "testfile.txt",
            ContentType = "text/plain",
            Size = 10,
            BlobUrl = "http://example.com/testfile.txt",
            UserId = Guid.Parse("7927DE88-792A-409D-B6A4-E9E677672CC9")
        };

        var mockUser = new CurrentUser("7927DE88-792A-409D-B6A4-E9E677672CC9", "user@test.com", []);

        userContextMock.Setup(u => u.GetCurrentUser()).Returns(mockUser);
        blobStorageServiceMock.Setup(b => b.UploadToBlobAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("http://example.com/testfile.txt");
        mapperMock.Setup(m => m.Map<FileDetail>(request)).Returns(fileDetail);

        var handler = new UploadFileCommandHandler(
            blobStorageServiceMock.Object,
            fileDetailRepositoryMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            userContextMock.Object
        );

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        blobStorageServiceMock.Verify(b => b.UploadToBlobAsync(request.FileName, request.File), Times.Once);
        fileDetailRepositoryMock.Verify(r => r.AddAsync(It.Is<FileDetail>(f => f.BlobUrl == "http://example.com/testfile.txt")), Times.Once);

        loggerMock.VerifyLogger(LogLevel.Information, $"Checking user on id: {request.userId}", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"Uploading file {request.FileName} to Blob Storage", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, "Saving file details to the repository", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"File uploaded and saved successfully with URL: {fileDetail.BlobUrl}", Times.Once());
    }

    [Fact]
    public async Task Handle_UserIdMismatch_ThrowsNotFoundException()
    {
        // Arrange
        var request = new UploadFileCommand 
        { 
            userId = Guid.NewGuid(), 
            FileName = "testfile.txt", 
            ContentType = "text/plain",
            Size = 10,
            File = new MemoryStream(new byte[] { 0x1, 0x2 }) 
        };

        var mockUser = new CurrentUser("7927DE88-792A-409D-B6A4-E9E677672CC9", "user@test.com", ["User"]);

        userContextMock.Setup(u => u.GetCurrentUser()).Returns(mockUser);

        var handler = new UploadFileCommandHandler(
            blobStorageServiceMock.Object,
            fileDetailRepositoryMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            userContextMock.Object
        );

        // Act
        var action = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with id: {request.userId} not found");
    }

    [Fact]
    public async Task Handle_BlobUploadFails_LogsErrorAndThrowsException()
    {
        // Arrange
        var request = new UploadFileCommand 
        { 
            userId = Guid.Parse("7927DE88-792A-409D-B6A4-E9E677672CC9"), 
            FileName = "testfile.txt", 
            File = new MemoryStream(new byte[] { 0x1, 0x2 }) 
        };

        var mockUser = new CurrentUser("7927DE88-792A-409D-B6A4-E9E677672CC9", "user@test.com", ["User"]);

        userContextMock.Setup(u => u.GetCurrentUser()).Returns(mockUser);
        blobStorageServiceMock.Setup(b => b.UploadToBlobAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ThrowsAsync(new Exception("Blob storage error"));

        var handler = new UploadFileCommandHandler(
            blobStorageServiceMock.Object,
            fileDetailRepositoryMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            userContextMock.Object
        );

        // Act 
        var action = () => handler.Handle(request, CancellationToken.None);

        //Assert
        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Blob storage error");

        loggerMock.VerifyLogger(LogLevel.Error, $"An error occurred while uploading the file {request.FileName}", Times.Once());
    }
}