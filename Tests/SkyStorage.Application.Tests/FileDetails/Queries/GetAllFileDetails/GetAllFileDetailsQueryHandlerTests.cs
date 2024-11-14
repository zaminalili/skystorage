using AutoMapper;
using FluentAssertions;
using Moq;
using SkyStorage.Application.FileDetails.Dtos;
using SkyStorage.Application.Pagination;
using SkyStorage.Application.Users;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Repositories;
using System.Reflection.Metadata;
using Xunit;

namespace SkyStorage.Application.FileDetails.Queries.GetAllFileDetails.Tests;

public class GetAllFileDetailsQueryHandlerTests
{
    private readonly Mock<IFileDetailRepository> fileDetailRepositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly GetAllFileDetailsQueryHandler handler;

    public GetAllFileDetailsQueryHandlerTests()
    {
        this.fileDetailRepositoryMock = new Mock<IFileDetailRepository>();
        this.mapperMock = new Mock<IMapper>();
        this.userContextMock = new Mock<IUserContext>();

        this.handler = new GetAllFileDetailsQueryHandler(
            fileDetailRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsPagedResult()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var currentUser = new CurrentUser(userId, "testuser@mail.com", []);
        userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var fileDetails = new List<FileDetail> 
        { 
            new FileDetail { Id = Guid.Parse("ADA8EA53-D4AB-4503-A005-DF669F717687"), FileName = "File1", ContentType = "text/plain", Size = 10, BlobUrl="http://example.com/testfile.txt", UserId = Guid.Parse("008B4421-76FA-49C0-B653-89C3D7F49268") } ,
            new FileDetail { Id = Guid.Parse("5824D1C9-2063-496A-960E-18F4F99B69EF"), FileName = "File2", ContentType = "text/plain", Size = 10, BlobUrl="http://example.com/testfile.txt", UserId = Guid.Parse("C1A2E742-4EB7-4823-8E2C-47392EF0364F") } ,
        };
        var totalCount = 10;
        fileDetailRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((fileDetails, totalCount));

        var fileDetailDtos = new List<FileDetailDto> 
        {
            new FileDetailDto { Id = Guid.Parse("ADA8EA53-D4AB-4503-A005-DF669F717687"), FileName = "File1", ContentType = "text/plain", Size = 10, BlobUrl="http://example.com/testfile.txt", UserId = Guid.Parse("008B4421-76FA-49C0-B653-89C3D7F49268") } ,
            new FileDetailDto { Id = Guid.Parse("5824D1C9-2063-496A-960E-18F4F99B69EF"), FileName = "File2", ContentType = "text/plain", Size = 10, BlobUrl="http://example.com/testfile.txt", UserId = Guid.Parse("C1A2E742-4EB7-4823-8E2C-47392EF0364F") } ,
        };

        mapperMock.Setup(x => x.Map<IEnumerable<FileDetailDto>>(fileDetails)).Returns(fileDetailDtos);

        var request = new GetAllFileDetailsQuery { searchPhrase = "", pageSize = 5, pageNumber = 1 };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        Xunit.Assert.Equal(fileDetailDtos, result.Items);
    }
}