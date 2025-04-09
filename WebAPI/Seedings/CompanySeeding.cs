using Domain.Companies;
using Infrastructure.DbContexts;

namespace WebApi.Seedings;

public sealed class CompanySeeding : ISeeding
{
    public static void SeedDatabase(MainDbContext context)
    {
        var companyName = "RootCompany";
        var saved = false;
        if (!context.Companies.Any(c => c.Name == companyName))
        {
            context.Companies.Add(Company.Create(companyName));
            context.SaveChanges();
        }
        if (!context.Users.Any(r => r.Name == "root")) {

            var roleId =  context.Roles.First(r => r.Name == "Admin").Id;
            var companyId = context.Companies.First(c => c.Name == companyName).Id;
            
            context.Users.Add(User.Create(roleId, companyId, "root", "Root000!"));
            saved = true;
        }
        if (!context.Users.Any(r => r.Name == "manager")) {

            var roleId =  context.Roles.First(r => r.Name == "Manager").Id;
            var companyId = context.Companies.First(c => c.Name == companyName).Id;
            
            context.Users.Add(User.Create(roleId, companyId, "manager", "Manager000!"));
            saved = true;
        }
        if (!context.Users.Any(r => r.Name == "recruiter")) {

            var roleId =  context.Roles.First(r => r.Name == "Recruiter").Id;
            var companyId = context.Companies.First(c => c.Name == companyName).Id;
            
            context.Users.Add(User.Create(roleId, companyId, "recruiter", "Recruiter000!"));
            saved = true;
        }
        if (saved)
        {
            context.SaveChanges();
        }
    }
}