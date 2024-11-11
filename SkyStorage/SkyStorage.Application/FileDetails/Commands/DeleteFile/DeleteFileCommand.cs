using MediatR;

namespace SkyStorage.Application.FileDetails.Commands.DeleteFile;

public class DeleteFileCommand(Guid fileId): IRequest
{
    public Guid FileId { get; set; } = fileId;
}
