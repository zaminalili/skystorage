using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SkyStorage.Application.Tests.Extensions;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;
using System.Reflection.Metadata;
using Xunit;

namespace SkyStorage.Application.FileDetails.Commands.DeleteFile.Tests;

public class DeleteFileCommandHandlerTests
{

    private readonly Mock<ILogger<DeleteFileCommandHandler>> loggerMock;
    private readonly Mock<IFileDetailRepository> fileDetailRepositoryMock;
    private readonly Mock<IBlobStorageService> blobStorageServiceMock;
    private readonly DeleteFileCommandHandler handler;

    public DeleteFileCommandHandlerTests()
    {
        this.loggerMock = new Mock<ILogger<DeleteFileCommandHandler>>();
        this.fileDetailRepositoryMock = new Mock<IFileDetailRepository>();
        this.blobStorageServiceMock = new Mock<IBlobStorageService>();

        this.handler = new DeleteFileCommandHandler(
            loggerMock.Object,
            fileDetailRepositoryMock.Object,
            blobStorageServiceMock.Object
        );
    }

    [Fact()]
    public async Task Handle_FileExists_ShouldDeleteFileFromRepositoryAndStorage()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var fileName = "testfile.txt";
        var command = new DeleteFileCommand(fileId);

        var fileDetail = new FileDetail
        {
            Id = fileId,
            FileName = fileName
        };

        fileDetailRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId))
            .ReturnsAsync(fileDetail);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        fileDetailRepositoryMock.Verify(repo => repo.DeleteAsync(fileId), Times.Once);
        blobStorageServiceMock.Verify(storage => storage.DeleteFileAsync(fileName), Times.Once);

        loggerMock.VerifyLogger(LogLevel.Information, $"Started deleting file with FileId: {fileId}", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"File with FileId: {fileId} found. Deleting from repository.", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"File with FileId: {fileId} deleted from repository. Deleting file from blob storage.", Times.Once());
        loggerMock.VerifyLogger(LogLevel.Information, $"File with FileId: {fileId} successfully deleted.", Times.Once());

    }


    [Fact]
    public async Task Handle_FileDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var command = new DeleteFileCommand(fileId);

        fileDetailRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId)).ReturnsAsync((FileDetail)null!);

        // Act & Assert
        var action = () => handler.Handle(command, CancellationToken.None);
        
        await action.Should()
           .ThrowAsync<NotFoundException>()
           .WithMessage($"fileDetail with id: {fileId} not found");
        
        fileDetailRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        blobStorageServiceMock.Verify(storage => storage.DeleteFileAsync(It.IsAny<string>()), Times.Never);
    }
}