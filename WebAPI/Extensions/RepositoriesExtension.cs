using Application.Candidates.Repository;
using Application.Companies.Repositories;
using Application.Vacancies.Repository;
using Infrastructure.Repositories;

namespace WebApi.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddScopedRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IVacancyRepository, VacancyRepository>();
        return services;
    }
}