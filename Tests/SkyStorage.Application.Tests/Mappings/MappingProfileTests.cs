using Xunit;
using AutoMapper;
using SkyStorage.Application.FileDetails.Commands.UploadFile;
using SkyStorage.Application.FileDetails.Dtos;
using SkyStorage.Domain.Entities;
using FluentAssertions;

namespace SkyStorage.Application.Mappings.Tests;

public class MappingProfileTests
{
    private readonly IMapper mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        this.mapper = config.CreateMapper();
    }

    [Fact()]
    public void Should_Map_FileDetail_To_FileDetailDto()
    {
        // Arrange
        var fileDetail = new FileDetail
        {
            Id = Guid.NewGuid(),
            FileName = "testfile.txt",
            Size = 1024,
            ContentType = "text/plain",
            BlobUrl = "https://testurl.com",
            UserId = Guid.NewGuid()
        };

        // Act
        var fileDetailDto = mapper.Map<FileDetailDto>(fileDetail);

        // Assert
        fileDetailDto.Should().NotBeNull();
        fileDetailDto.Id.Should().Be(fileDetail.Id);
        fileDetailDto.FileName.Should().Be(fileDetail.FileName);
        fileDetailDto.Size.Should().Be(fileDetail.Size);
        fileDetailDto.ContentType.Should().Be(fileDetail.ContentType);
        fileDetailDto.UserId.Should().Be(fileDetail.UserId);
        fileDetailDto.BlobUrl.Should().Be(fileDetail.BlobUrl);
    }

    [Fact()]
    public void Should_Map_FileDetailDto_To_FileDetail()
    {
        // Arrange
        var fileDetailDto = new FileDetailDto
        {
            Id = Guid.NewGuid(),
            FileName = "testfile.txt",
            Size = 1024,
            ContentType = "text/plain",
            BlobUrl = "https://testurl.com",
            UserId = Guid.NewGuid()
        };

        // Act
        var fileDetail = mapper.Map<FileDetail>(fileDetailDto);

        // Assert
        fileDetail.Should().NotBeNull();
        fileDetail.Id.Should().Be(fileDetailDto.Id);
        fileDetail.FileName.Should().Be(fileDetailDto.FileName);
        fileDetail.Size.Should().Be(fileDetailDto.Size);
        fileDetail.ContentType.Should().Be(fileDetailDto.ContentType);
        fileDetail.UserId.Should().Be(fileDetailDto.UserId);
        fileDetail.BlobUrl.Should().Be(fileDetailDto.BlobUrl);
    }

    [Fact()]
    public void Should_Map_UploadFileCommand_To_FileDetail()
    {
        // Arrange
        var uploadFileCommand = new UploadFileCommand
        {
            userId = Guid.NewGuid(),
            FileName = "uploadfile.txt",
            Size = 1024,
            ContentType = "text/plain"
        };

        // Act
        var fileDetail = mapper.Map<FileDetail>(uploadFileCommand);

        // Assert
        fileDetail.UserId.Should().Be(uploadFileCommand.userId);
        fileDetail.FileName.Should().Be(uploadFileCommand.FileName);
        fileDetail.Size.Should().Be(uploadFileCommand.Size);
        fileDetail.ContentType.Should().Be(uploadFileCommand.ContentType);
    }
}