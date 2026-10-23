using Api.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace Api.BLL.Context;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    DbSet<Auth> Auths => Set<Auth>();
}