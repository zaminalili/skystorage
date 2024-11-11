using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Infrastructure.Configuration;
using System.IO;

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

    public async Task<(Stream, string, string)> DownloadFileAsync(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.FilesContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0;
            //Stream blobStream = blobClient.OpenReadAsync().Result;

            var contentType = blobClient.GetProperties().Value.ContentType;
            var name = blobClient.Name;

            return (memoryStream, contentType, name);
        }
        else
        {
            throw new FileNotFoundException($"Fayl tapılmadı: {fileName}");
        }
    }
}
