namespace SkyStorage.Application.Users;

public interface ICurrentUserValidator
{
    bool IsCurrentUser(Guid userId);
}
