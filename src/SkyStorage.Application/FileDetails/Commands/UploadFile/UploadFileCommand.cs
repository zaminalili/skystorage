using MediatR;

namespace SkyStorage.Application.FileDetails.Commands.UploadFile;

public class UploadFileCommand: IRequest
{
    public Guid userId { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public Stream File { get; set; } = default!;
}
