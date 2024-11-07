namespace SkyStorage.Domain.Entities;

public class FileDetail
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public string BlobUrl { get; set; } = default!;
    public DateTime UploadedDate { get; set; } = DateTime.Now;
    public DateTime LastModifiedDate { get; set; }

    // foreign key və navigation property
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
