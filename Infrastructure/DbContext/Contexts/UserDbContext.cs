using Domain.Companies;

namespace Infrastructure.DbContext.Contexts;

using Microsoft.EntityFrameworkCore;

public class UserDbContext :DbContext
{
    public DbSet<User> Users { get; set; }
}