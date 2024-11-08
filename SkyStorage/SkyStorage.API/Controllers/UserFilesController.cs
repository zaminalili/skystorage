using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyStorage.Application.FileDetails.Queries.GetAllFileDetails;
using SkyStorage.Application.Users;

namespace SkyStorage.API.Controllers
{
    [Route("api/users/{userId}/files")]
    [ApiController]
    [Authorize]
    public class UserFilesController(IMediator mediator, ICurrentUserValidator userValidator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllFileDetails([FromRoute] Guid userId, [FromQuery] GetAllFileDetailsQuery query)
        {
            if(userValidator.IsCurrentUser(userId))
            {
                var fileDetails = await mediator.Send(query);
                return Ok(fileDetails);
            }

            return BadRequest();
        }
    }
}
