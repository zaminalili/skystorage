using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using SkyStorage.Domain.Interfaces;
using SkyStorage.Infrastructure.Configuration;
using System.IO;

namespace SkyStorage.Infrastructure.Storage;

internal class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient containerClient;

    public BlobStorageService(IOptions<BlobStorageSettings> options)
    {
        BlobStorageSettings settings = options.Value;

        var blobServiceClient = new BlobServiceClient(settings.ConnectionString)
            ?? throw new ArgumentNullException(nameof(settings.ConnectionString));

        containerClient = blobServiceClient.GetBlobContainerClient(settings.FilesContainerName);
    }

    public async Task<string> UploadToBlobAsync(string fileName, Stream file)
    {
       
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(file);

        var blobUrl = blobClient.Uri.ToString(); 
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

            return (memoryStream, contentType, name);
        }
        else
        {
            throw new FileNotFoundException($"Fayl tapılmadı: {fileName}");
        }
    }
}
