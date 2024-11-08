using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyStorage.Application.Users.Queries.GetCurrentUserId;

namespace SkyStorage.API.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpGet("currentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUser = await mediator.Send(new GetCurrentUserIdQuery());

            return Ok(currentUser);
        }
    }
}
