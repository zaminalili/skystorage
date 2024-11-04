using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyStorage.Infrastructure.Persistence;

namespace SkyStorage.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        string connectionStringName = "SkyStorageLocalDb";
        var connectionString = configuration.GetConnectionString(connectionStringName);

        services.AddDbContext<SkyStorageDbContext>(options => options.UseSqlServer(connectionString));
    }
}
