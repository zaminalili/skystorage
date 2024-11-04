using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkyStorage.Domain.Entities;

namespace SkyStorage.Infrastructure.Persistence;

internal class SkyStorageDbContext(DbContextOptions<SkyStorageDbContext> options): 
    IdentityDbContext<User, Role, Guid, 
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken> (options)
{

}
