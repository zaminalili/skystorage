using SkyStorage.Domain.Exceptions;

namespace SkyStorage.Application.Users;

internal class CurrentUserValidator(IUserContext userContext) : ICurrentUserValidator
{
    public bool IsCurrentUser(Guid userId)
    {
        if (userId.ToString() != userContext.GetCurrentUser()!.Id)
            throw new MismatchException("User", "Current user");

        return true;
    }
}
