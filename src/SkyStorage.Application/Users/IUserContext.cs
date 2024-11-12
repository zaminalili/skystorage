namespace SkyStorage.Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
