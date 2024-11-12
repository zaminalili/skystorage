using FluentValidation;

namespace SkyStorage.Application.FileDetails.Queries.GetAllFileDetails;

public class GetAllFileDetailsQueryValidator: AbstractValidator<GetAllFileDetailsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 20, 25];
    public GetAllFileDetailsQueryValidator()
    {
        RuleFor(b => b.pageNumber).GreaterThanOrEqualTo(1);

        RuleFor(b => b.pageSize)
            .Must(v => allowPageSizes.Contains(v))
            .WithMessage($"Page size must be in {string.Join(",", allowPageSizes)}");
    }
}
