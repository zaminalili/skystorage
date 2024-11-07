using Microsoft.EntityFrameworkCore;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Repositories;
using SkyStorage.Infrastructure.Persistence;

namespace SkyStorage.Infrastructure.Repositories;

internal class FileDetailRepository(SkyStorageDbContext dbContext) : IFileDetailRepository
{
    public async Task AddAsync(FileDetail fileDetail)
    {
        await dbContext.FileDetails.AddAsync(fileDetail);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var fileDetail = await GetByIdAsync(id);
        if (fileDetail != null)
        {
            dbContext.FileDetails.Remove(fileDetail);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<FileDetail>> GetAllAsync()
    {
        return await dbContext.FileDetails.ToListAsync();
    }

    public async Task<FileDetail?> GetByIdAsync(Guid id)
    {
        return await dbContext.FileDetails.FindAsync(id);
    }

    public async Task UpdateAsync(FileDetail fileDetail)
    {
        dbContext.FileDetails.Update(fileDetail);
        await dbContext.SaveChangesAsync();
    }
}
