using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SkyStorage.Application.FileDetails.Queries.DownloadFile;
using SkyStorage.Application.Tests.Extensions;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;
using System;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using Xunit;

namespace SkyStorage.Application.Tests.FileDetails.Queries.DownloadFile;

public class DownloadFileQueryHandlerTests
{
    private readonly Mock<IBlobStorageService> blobStorageServiceMock;
    private readonly Mock<IFileDetailRepository> fileDetailRepositoryMock;
    private readonly Mock<ILogger<DownloadFileQueryHandler>> loggerMock;
    private readonly DownloadFileQueryHandler handler;

    public DownloadFileQueryHandlerTests()
    {
        this.blobStorageServiceMock = new Mock<IBlobStorageService>();
        this.fileDetailRepositoryMock = new Mock<IFileDetailRepository>();
        this.loggerMock = new Mock<ILogger<DownloadFileQueryHandler>>();

        this.handler = new DownloadFileQueryHandler(
            blobStorageServiceMock.Object,
            fileDetailRepositoryMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task Handle_FileExists_ReturnsFileStreamAndMetadata()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var fileDetail = new FileDetail 
        { 
            FileName = "example.txt" 
        };
        var fileStream = new MemoryStream();
        var contentType = "text/plain";
        var fileName = "example.txt";

        fileDetailRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId))
            .ReturnsAsync(fileDetail);

        blobStorageServiceMock.Setup(service => service.DownloadFileAsync(fileDetail.FileName))
            .ReturnsAsync((fileStream, contentType, fileName));

        var request = new DownloadFileQuery(fileId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(fileStream, result.Item1);
        result.Item2.Should().Be(contentType);
        result.Item3.Should().Be(fileName);
        

        loggerMock.VerifyLogger(LogLevel.Information, $"Processing download request for FileId: {fileId}", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"File found for FileId: {fileId}. Starting download...", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"Download completed for FileId: {fileId}", Times.Once());
    }

    [Fact]
    public async Task Handle_FileNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        fileDetailRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId))
            .ReturnsAsync((FileDetail)null!);

        var request = new DownloadFileQuery(fileId);

        // Act
        var action = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"fileDetail with id: {fileId} not found");


        loggerMock.VerifyLogger(LogLevel.Information, $"Processing download request for FileId: {fileId}", Times.Once());
    }
}