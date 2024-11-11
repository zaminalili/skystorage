using MediatR;
using Microsoft.Extensions.Logging;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;

namespace SkyStorage.Application.FileDetails.Commands.DeleteFile;

public class DeleteFileCommandHandler(ILogger<DeleteFileCommandHandler> logger, IFileDetailRepository fileDetailRepository, IBlobStorageService blobStorageService) : IRequestHandler<DeleteFileCommand>
{
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started deleting file with FileId: {FileId}", request.FileId);

        var fileDetail = await fileDetailRepository.GetByIdAsync(request.FileId);

        if (fileDetail == null)
            throw new NotFoundException(nameof(fileDetail), request.FileId.ToString());

        logger.LogInformation("File with FileId: {FileId} found. Deleting from repository.", request.FileId);

        await fileDetailRepository.DeleteAsync(request.FileId);

        logger.LogInformation("File with FileId: {FileId} deleted from repository. Deleting file from blob storage.", request.FileId);

        await blobStorageService.DeleteFileAsync(fileDetail.FileName);

        logger.LogInformation("File with FileId: {FileId} successfully deleted.", request.FileId);
    }
}
