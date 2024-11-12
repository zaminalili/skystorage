using AutoMapper;
using MediatR;
using SkyStorage.Application.FileDetails.Dtos;
using SkyStorage.Application.Pagination;
using SkyStorage.Application.Users;
using SkyStorage.Domain.Exceptions;
using SkyStorage.Domain.Repositories;

namespace SkyStorage.Application.FileDetails.Queries.GetAllFileDetails;

public class GetAllFileDetailsQueryHandler(IFileDetailRepository fileDetailRepository, IMapper mapper, IUserContext userContext) : IRequestHandler<GetAllFileDetailsQuery, PagedResult<FileDetailDto>>
{
    public async Task<PagedResult<FileDetailDto>> Handle(GetAllFileDetailsQuery request, CancellationToken cancellationToken)
    {
        Guid userId = new Guid(userContext.GetCurrentUser()!.Id);
        if (userId.ToString() != userContext.GetCurrentUser()!.Id)
            throw new NotFoundException("User", userId.ToString());

        var (fileDetails, totalCount) = await fileDetailRepository.GetAllAsync(userId, request.searchPhrase, request.pageSize, request.pageNumber);

        var fileDetailDtos = mapper.Map<IEnumerable<FileDetailDto>>(fileDetails);
        var result = new PagedResult<FileDetailDto>(fileDetailDtos, totalCount, request.pageSize, request.pageNumber);

        return result;
    }
}
