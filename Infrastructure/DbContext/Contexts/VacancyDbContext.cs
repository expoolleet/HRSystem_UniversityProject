using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext.Contexts;

public class VacancyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Vacancy> Vacancies { get; set; }
}