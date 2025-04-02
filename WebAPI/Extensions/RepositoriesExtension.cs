using Application.Candidates.Repositories;
using Application.Companies.Repositories;
using Application.Vacancies.Repositories;
using Infrastructure.Repositories;

namespace WebApi.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddScopedRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IVacancyRepository, VacancyRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        return services;
    }
}