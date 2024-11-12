using MediatR;

namespace SkyStorage.Application.Users.Queries.GetCurrentUserId;

public class GetCurrentUserIdQuery: IRequest<CurrentUser?>
{
}
