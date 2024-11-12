using MediatR;

namespace SkyStorage.Application.FileDetails.Queries.DownloadFile;

public class DownloadFileQuery(Guid fileId): IRequest<(Stream, string, string)>
{
    public Guid FileId { get; set; } = fileId;
}
