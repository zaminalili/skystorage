using Microsoft.Extensions.DependencyInjection;
using SkyStorage.Application.Users;

namespace SkyStorage.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUserValidator, UserValidator>();
    }
}
