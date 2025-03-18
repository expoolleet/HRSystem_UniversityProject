using Domain.Companies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext.Contexts;

public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<User> Users { get; set; }
}