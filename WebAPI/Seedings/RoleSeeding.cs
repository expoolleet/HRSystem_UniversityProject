using Domain.Companies;
using Infrastructure.DbContexts;

namespace WebApi.Seedings;

public sealed class RoleSeeding : ISeeding
{
    public static void SeedDatabase(MainDbContext context)
    {
        var added = false;
        if (!context.Roles.Any(r => r.Name == "Admin")) {
            context.Roles.Add(Role.Create("Admin"));
            added = true;
        }
        if (!context.Roles.Any(r => r.Name == "Manager")) {
            context.Roles.Add(Role.Create("Manager"));
            added = true;
        }
        if (!context.Roles.Any(r => r.Name == "Recruiter")) {
            context.Roles.Add(Role.Create("Recruiter"));
            added = true;
        }
        if(added) {
            context.SaveChanges();
        }
    }
}