namespace SkyStorage.Infrastructure.MigrationTasks;

public interface IMigrationRunner
{
    Task Migrate();
}
