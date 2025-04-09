using WebApi.Contracts.MappingProfiles.Candidates;
using WebApi.Contracts.MappingProfiles.Companies;
using WebApi.Contracts.MappingProfiles.Vacancies;

namespace WebApi.Extensions;

public static class AutoMappersExtension
{
    public static IServiceCollection AddAutoMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CandidateMappingProfile));
        services.AddAutoMapper(typeof(CandidateDocumentMappingProfile));
        
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(RoleMappingProfile));
        services.AddAutoMapper(typeof(TokenMappingProfile));
        services.AddAutoMapper(typeof(UserResponseMappingProfile));
        
        services.AddAutoMapper(typeof(VacancyMappingProfile));
        services.AddAutoMapper(typeof(VacancyShortMappingProfile));
        services.AddAutoMapper(typeof(VacancyWorkflowMappingProfile));
        return services;
    }
}