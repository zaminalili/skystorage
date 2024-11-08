using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyStorage.Domain.Entities;
using SkyStorage.Domain.Repositories;
using SkyStorage.Infrastructure.MigrationTasks;
using SkyStorage.Infrastructure.Persistence;
using SkyStorage.Infrastructure.Repositories;


namespace SkyStorage.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IdentityBuilder identityBuilder)
    {
        
        string connectionStringName = "SkyStorageLocalDb";
        var connectionString = configuration.GetConnectionString(connectionStringName);

        services.AddDbContext<SkyStorageDbContext>(options => options.UseSqlServer(connectionString));

       identityBuilder.AddEntityFrameworkStores<SkyStorageDbContext>();

        services.AddScoped<IFileDetailRepository, FileDetailRepository>();
        services.AddScoped<IMigrationRunner, MigrationRunner>();
    }
}
