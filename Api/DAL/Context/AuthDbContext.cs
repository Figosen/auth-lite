using Api.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace Api.BLL.Context;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<Auth> Auths => Set<Auth>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auth>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Auth>().HasIndex(a => a.RefreshTokenHash).IsUnique();
    }
}