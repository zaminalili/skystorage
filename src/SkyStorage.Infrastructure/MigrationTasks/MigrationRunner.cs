
using Microsoft.EntityFrameworkCore;
using SkyStorage.Infrastructure.Persistence;

namespace SkyStorage.Infrastructure.MigrationTasks;

internal class MigrationRunner(SkyStorageDbContext dbContext) : IMigrationRunner
{
    public async Task Migrate()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
