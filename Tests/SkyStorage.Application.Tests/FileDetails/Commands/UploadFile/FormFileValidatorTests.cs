using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SkyStorage.Application.FileDetails.Commands.UploadFile.Tests;

public class FormFileValidatorTests
{
    private readonly FormFileValidator validator;

    public FormFileValidatorTests()
    {
        this.validator = new FormFileValidator();
    }

    private IFormFile CreateMockFormFile(long sizeInBytes)
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(sizeInBytes);
        return fileMock.Object;
    }


    [Fact()]
    public void Should_HaveError_WhenFileExceedsMaxSize()
    {
        // Arrange
        var oversizedFile = CreateMockFormFile(11 * 1024 * 1024); // 11 MB

        // Act
        var result = validator.TestValidate(oversizedFile);

        // Assert
        result.ShouldHaveValidationErrorFor(file => file)
              .WithErrorMessage("The maximum file size is 10 MB.");
    }

    [Fact()]
    public void Should_NotHaveError_WhenFileIsValid()
    {
        // Arrange
        var validFile = CreateMockFormFile(5 * 1024 * 1024); // 5 MB

        // Act
        var result = validator.TestValidate(validFile);

        // Assert
        result.ShouldNotHaveValidationErrorFor(file => file);
    }
}