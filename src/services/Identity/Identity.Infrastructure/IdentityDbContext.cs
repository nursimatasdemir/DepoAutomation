using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Identity.Infrastructure;

public class IdentityDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        Guid ADMIN_ROLE_ID = Guid.Parse("a18be9c0-aa65-4af8-bd17-00bd9344e575");
        Guid OPERATOR_ROLE_ID = Guid.Parse("a28be9c0-aa65-4af8-bd17-00bd9344e575");

        builder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new IdentityRole<Guid>
            {
                Id = OPERATOR_ROLE_ID,
                Name = "Operator",
                NormalizedName = "OPERATOR"
            }
        );
    }
}