using SkyStorage.Domain.Entities;

namespace SkyStorage.Domain.Repositories;

public interface IFileDetailRepository
{
    Task<FileDetail?> GetByIdAsync(Guid id);
    Task<(IEnumerable<FileDetail>, int)> GetAllAsync(Guid userId, string? searchPhrase, int pageSize, int pageNumber);
    Task AddAsync(FileDetail fileDetail);
    Task UpdateAsync(FileDetail fileDetail);
    Task DeleteAsync(Guid id);
}

