using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Domain.Repositories;

namespace SkyStorage.Application.FileDetails.Commands.UploadFile;

public class UploadFileCommandHandler(IBlobStorageService blobStorageService, 
                                      IFileDetailRepository fileDetailRepository, 
                                      IMapper mapper,
                                      ILogger<UploadFileCommandHandler> logger) : IRequestHandler<UploadFileCommand>
{
    public async Task Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Uploading file {FileName} to Blob Storage", request.FileName);
            var fileUrl = blobStorageService.UploadToBlobAsync(request.FileName, request.File);

            var fileDetail = mapper.Map<FileDetail>(request);
            fileDetail.BlobUrl = await fileUrl;

            logger.LogInformation("Saving file details to the repository");
            await fileDetailRepository.AddAsync(fileDetail);

            logger.LogInformation("File uploaded and saved successfully with URL: {BlobUrl}", fileDetail.BlobUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while uploading the file {FileName}", request.FileName);
            throw;
        }

       
    }
}
