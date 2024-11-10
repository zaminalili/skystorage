using FluentValidation;
using SkyStorage.Domain.Exceptions;

namespace SkyStorage.Application.Users;

public class CurrentUserValidator: AbstractValidator<Guid>
{
    public CurrentUserValidator(IUserContext userContext)
    {
        Guid currentUserId = new Guid(userContext.GetCurrentUser()!.Id);

        RuleFor(g => g)
            .Equal(currentUserId)
            .WithMessage("ID does not match the current user ID");
    }
}
