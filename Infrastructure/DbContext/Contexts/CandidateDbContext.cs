using Domain.Candidates;
using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext.Contexts;

public class CandidateDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Candidate> Candidates { get; set; }
    
    public DbSet<Vacancy> Vacancies { get; set; }
}