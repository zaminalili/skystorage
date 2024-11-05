using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SkyStorage.Application.Users;

public class UserContext(IHttpContextAccessor httpContextAccessor, IUserValidator userValidator) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null)
            throw new InvalidOperationException("User context is not present");

        if (!userValidator.IsUserAuthenticated(user))
            return null;

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

        return new CurrentUser(userId, email, roles);
    }
}
