namespace SkyStorage.Application.FileDetails.Dtos;

public class FileDetailDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public string BlobUrl { get; set; } = default!;
    public DateTime UploadedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public Guid UserId { get; set; }
}
