using Microsoft.AspNetCore.Identity;

namespace SkyStorage.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public ICollection<FileDetail> FileDetails { get; set; } = default!;
}
