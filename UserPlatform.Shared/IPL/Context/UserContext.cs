using Microsoft.EntityFrameworkCore;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.IPL.Context;

public sealed class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {        
    }

    internal DbSet<User> Users { get; set; }
    internal DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.ComplexProperty(e => e.Location);
            e.ComplexProperty(e => e.Contact);
            e.OwnsMany(e => e.RefreshTokens);
        });

        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.ComplexProperty(e => e.User);
        });
    }

}
