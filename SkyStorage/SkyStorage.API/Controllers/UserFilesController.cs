using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyStorage.Application.FileDetails.Commands.UploadFile;
using SkyStorage.Application.FileDetails.Queries.GetAllFileDetails;
using SkyStorage.Application.Users;

namespace SkyStorage.API.Controllers
{
    [Route("api/users/{userId}/files")]
    [ApiController]
    [Authorize]
    public class UserFilesController(IMediator mediator, IValidator<IFormFile> fileValidator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllFileDetails([FromRoute] Guid userId, [FromQuery] GetAllFileDetailsQuery query)
        {

            var fileDetails = await mediator.Send(query);
            return Ok(fileDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromRoute] Guid userId, IFormFile file)
        {
            var validationResult = fileValidator.Validate(file);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors[0].ErrorMessage);


            using var stream = file.OpenReadStream();

            var command = new UploadFileCommand()
            {
                userId = userId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                File = stream
            };

            await mediator.Send(command);

            return NoContent();
        }
    }
}
