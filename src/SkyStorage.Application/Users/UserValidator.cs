using System.Security.Claims;

namespace SkyStorage.Application.Users;

internal class UserValidator : IUserValidator
{
    public bool IsUserAuthenticated(ClaimsPrincipal? user)
    {
        return user?.Identity != null && user.Identity.IsAuthenticated;
    }
}
