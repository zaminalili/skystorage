using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Infrastructure.Configuration;
using System.IO;

namespace SkyStorage.Infrastructure.Storage;

internal class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient containerClient;
    private readonly ILogger<BlobStorageService> logger;

    public BlobStorageService(IOptions<BlobStorageSettings> options, ILogger<BlobStorageService> logger)
    {
        this.logger = logger;
        BlobStorageSettings settings = options.Value;

        var blobServiceClient = new BlobServiceClient(settings.ConnectionString)
            ?? throw new ArgumentNullException(nameof(settings.ConnectionString));

        containerClient = blobServiceClient.GetBlobContainerClient(settings.FilesContainerName);
        this.logger.LogInformation("BlobStorageService initialized with container: {ContainerName}", settings.FilesContainerName);
    }

    public async Task<string> UploadToBlobAsync(string fileName, Stream file)
    {
       
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(file);

        var blobUrl = blobClient.Uri.ToString();
        logger.LogInformation("File uploaded to blob storage. FileName: {FileName}, BlobUrl: {BlobUrl}", fileName, blobUrl);

        return blobUrl;
    }
     
    public async Task<(Stream, string, string)> DownloadFileAsync(string fileName)
    {
        var blobClient = containerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0;

            var contentType = blobClient.GetProperties().Value.ContentType;
            var name = blobClient.Name;

            logger.LogInformation("File downloaded from blob storage. FileName: {FileName}", fileName);

            return (memoryStream, contentType, name);
        }
        else
        {
            throw new FileNotFoundException($"file {fileName} named not found");
        }
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var blobClient = containerClient.GetBlobClient(fileName);


        if (await blobClient.DeleteIfExistsAsync())
            logger.LogInformation("File deleted from blob storage. FileName: {FileName}", fileName);
        else
            throw new FileNotFoundException($"file {fileName} named not found");
    }
}
