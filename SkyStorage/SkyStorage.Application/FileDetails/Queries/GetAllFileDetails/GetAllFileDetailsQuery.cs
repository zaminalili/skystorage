using MediatR;
using SkyStorage.Application.FileDetails.Dtos;
using SkyStorage.Application.Pagination;

namespace SkyStorage.Application.FileDetails.Queries.GetAllFileDetails;

public class GetAllFileDetailsQuery: IRequest<PagedResult<FileDetailDto>>
{
    public string? searchPhrase { get; set; }
    public int pageSize { get; set; } = 10;
    public int pageNumber { get; set; } = 1;
}
