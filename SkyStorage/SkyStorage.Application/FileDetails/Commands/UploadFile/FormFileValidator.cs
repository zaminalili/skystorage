using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace SkyStorage.Application.FileDetails.Commands.UploadFile;

public class FormFileValidator: AbstractValidator<IFormFile>
{
    private long maxFileSizeMB = 10;
    public FormFileValidator()
    {
        RuleFor(file => file)
             .NotNull()
             .WithMessage("File should be uploaded.")
             .Must(file => file.Length <= maxFileSizeMB * 1024 * 1024)
             .WithMessage($"The maximum file size is {maxFileSizeMB} MB.");
    }
}
