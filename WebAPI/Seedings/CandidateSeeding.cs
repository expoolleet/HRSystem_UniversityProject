using Domain.Candidates;
using Infrastructure.DbContexts;

namespace WebApi.Seedings;

public class CandidateSeeding : ISeeding
{
    public static void SeedDatabase(MainDbContext context)
    {
        if (!context.Candidates.Any())
        {
            var vacancy1 = context.Vacancies.FirstOrDefault();
            var vacancy2 =  context.Vacancies.Skip(1).FirstOrDefault();

            if (vacancy1 != null && vacancy2 != null)
            {
                context.Candidates.AddRange(
                    vacancy1.CreateCandidate(CandidateDocument.Create("Candidate 1", 1, "Portfolio"), null),
                    vacancy1.CreateCandidate(CandidateDocument.Create("Candidate 2", 1, "Portfolio"), null),
                    vacancy2.CreateCandidate(CandidateDocument.Create("Candidate 3", 1, "Portfolio"), null)
                );
                context.SaveChanges();
            }
        }
    }
}