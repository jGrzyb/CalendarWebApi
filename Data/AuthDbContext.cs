using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace projekt.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var adminRoleId = Guid.NewGuid().ToString();
        var userRoleId = Guid.NewGuid().ToString();

        var roles = new List<IdentityRole>
        {
            new()
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },
            new()
            {
                Name = "User",
                NormalizedName = "User",
                Id = userRoleId,
                ConcurrencyStamp = userRoleId
            },
        };

        builder.Entity<IdentityRole>().HasData(roles);

        // create admin
        var adminId = Guid.NewGuid().ToString();
        var adminUser = new IdentityUser
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            NormalizedEmail = "admin@gmail.com".ToUpper(),
            NormalizedUserName = "admin@gmail.com".ToUpper(),
            Id = adminId
        };

        adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "admin");

        builder.Entity<IdentityUser>().HasData(adminUser);

        var adminRoles = new List<IdentityUserRole<string>>
        {
            new()
            {
                RoleId = adminRoleId,
                UserId = adminId
            },
            new()
            {
                RoleId = userRoleId,
                UserId = adminId
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}