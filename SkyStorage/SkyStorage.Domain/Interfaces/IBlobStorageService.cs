namespace SkyStorage.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(string fileName, Stream file);
}
