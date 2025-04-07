using Domain.Companies;
using Infrastructure.DbContexts;

namespace WebApi.Seedings;

public sealed class CompanySeeding : ISeeding
{
    public static void SeedDatabase(MainDbContext context)
    {
        var companyName = "RootCompany";
        if (!context.Companies.Any(c => c.Name == companyName))
        {
            context.Companies.Add(Company.Create(companyName));
            context.SaveChanges();
        }
        if (!context.Users.Any(r => r.Name == "root")) {

            var roleId =  context.Roles.First(r => r.Name == "Admin").Id;
            var companyId = context.Companies.First(c => c.Name == companyName).Id;
            
            context.Users.Add(User.Create(roleId, companyId, "root", "Root000!"));
            context.SaveChanges();
        }
    }
}