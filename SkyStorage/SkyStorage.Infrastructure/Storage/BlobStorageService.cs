using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Infrastructure.Configuration;

namespace SkyStorage.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> options) : IBlobStorageService
{
    private readonly BlobStorageSettings _settings = options.Value;

    public async Task<string> UploadToBlobAsync(string fileName, Stream file)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.FilesContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(file);

        var blobUrl = blobClient.Uri.ToString(); 
        return blobUrl;
    }
}
