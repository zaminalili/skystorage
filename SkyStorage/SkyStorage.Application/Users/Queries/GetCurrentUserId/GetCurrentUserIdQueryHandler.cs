using MediatR;

namespace SkyStorage.Application.Users.Queries.GetCurrentUserId;

public class GetCurrentUserIdQueryHandler(IUserContext userContext) : IRequestHandler<GetCurrentUserIdQuery, CurrentUser?>
{
    public Task<CurrentUser?> Handle(GetCurrentUserIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userContext.GetCurrentUser());
    }


}
