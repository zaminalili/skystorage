using System.Security.Claims;

namespace SkyStorage.Application.Users;

public interface IUserValidator
{
    bool IsUserAuthenticated(ClaimsPrincipal? user);
}
