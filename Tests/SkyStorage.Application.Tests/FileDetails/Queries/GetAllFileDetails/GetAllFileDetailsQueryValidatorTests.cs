using Xunit;
using FluentValidation.TestHelper;

namespace SkyStorage.Application.FileDetails.Queries.GetAllFileDetails.Tests;

public class GetAllFileDetailsQueryValidatorTests
{
    private readonly GetAllFileDetailsQueryValidator validator;
    public GetAllFileDetailsQueryValidatorTests()
    {
        this.validator = new GetAllFileDetailsQueryValidator();
    }

    [Fact()]
    public void Should_NotHaveError_ForValidQuery()
    {
        // arrange
        var query = new GetAllFileDetailsQuery()
        {
            searchPhrase = "",
            pageNumber = 1,
            pageSize = 10,
        };

        // act
        var result = validator.TestValidate(query);

        // asset
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Should_HaveError_ForInvalidQuery()
    {
        // arrange
        var query = new GetAllFileDetailsQuery()
        {
            searchPhrase = "",
            pageNumber = 2,
            pageSize = 2,
        };

        // act
        var result = validator.TestValidate(query);

        // asset
        result.ShouldHaveAnyValidationError();
    }
}