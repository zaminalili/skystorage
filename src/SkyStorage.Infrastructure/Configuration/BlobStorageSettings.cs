namespace SkyStorage.Infrastructure.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = default!;
    public string FilesContainerName { get; set; } = default!;
}
