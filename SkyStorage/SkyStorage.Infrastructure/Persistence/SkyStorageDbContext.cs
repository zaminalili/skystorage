using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkyStorage.Domain.Entities;

namespace SkyStorage.Infrastructure.Persistence;

internal class SkyStorageDbContext(DbContextOptions<SkyStorageDbContext> options): 
    IdentityDbContext<User, Role, Guid, 
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken> (options)
{
    public DbSet<FileDetail> FileDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<User>()
            .HasMany(u => u.FileDetails)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
