using MediatR;
using Microsoft.Extensions.Logging;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;

namespace SkyStorage.Application.FileDetails.Queries.DownloadFile;

public class DownloadFileQueryHandler(IBlobStorageService blobStorageService, 
                                        IFileDetailRepository fileDetailRepository,
                                        ILogger<DownloadFileQueryHandler> logger) : IRequestHandler<DownloadFileQuery, (Stream, string, string)>
{
    public async Task<(Stream, string, string)> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing download request for FileId: {FileId}", request.FileId);
        var fileDetail = await fileDetailRepository.GetByIdAsync(request.FileId);

        if (fileDetail == null)
            throw new NotFoundException(nameof(fileDetail), request.FileId.ToString());

        logger.LogInformation("File found for FileId: {FileId}. Starting download...", request.FileId);

        var (fileStream, contentType, name) = await blobStorageService.DownloadFileAsync(fileDetail.FileName);
        logger.LogInformation("Download completed for FileId: {FileId}", request.FileId);

        return (fileStream, contentType, name);
    }
}
