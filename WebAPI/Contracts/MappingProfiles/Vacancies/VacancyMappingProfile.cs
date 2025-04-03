using AutoMapper;
using Domain.Vacancies;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApi.Contracts.MappingProfiles.Vacancies;

public class VacancyMappingProfile : Profile
{
    public VacancyMappingProfile()
    {
        CreateMap<Vacancy, VacancyDto>().ReverseMap();
    }
}