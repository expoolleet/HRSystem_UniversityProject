using Domain.Vacancies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Seedings;

public sealed class VacancySeeding : ISeeding
{
    public static void SeedDatabase(MainDbContext context)
    {
        if (!context.Companies.Any())
        {
            return;
        }
        var user = context.Users.FirstAsync(u => u.Name == "root");
        Task.WaitAll(user);
        if (!context.Vacancies.Any(c => c.Description != null && c.Description.Contains("Test Vacancy 1")))
        {
            var steps = new List<VacancyWorkflowStep> { VacancyWorkflowStep.Create(user.Result.Id, context.Roles.First().Id, "Step 1", 1) };
            var workflow = VacancyWorkflow.Create(steps);
            context.Vacancies.Add(Vacancy.Create(context.Companies.First().Id, "Test Vacancy 1", workflow));
            context.SaveChanges();
        }
        if (!context.Vacancies.Any(c => c.Description != null && c.Description.Contains("Test Vacancy 2")))
        {
            var steps = new List<VacancyWorkflowStep> { VacancyWorkflowStep.Create(user.Result.Id, context.Roles.First().Id, "Step 1", 1) };
            var workflow = VacancyWorkflow.Create(steps);
            context.Vacancies.Add(Vacancy.Create(context.Companies.First().Id, "Test Vacancy 2", workflow));
            context.SaveChanges();
        }
    }
}