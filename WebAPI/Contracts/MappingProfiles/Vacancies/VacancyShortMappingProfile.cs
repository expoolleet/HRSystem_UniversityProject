using AutoMapper;
using Domain.Vacancies;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApi.Contracts.MappingProfiles.Vacancies;

public class VacancyShortMappingProfile : Profile
{
    public VacancyShortMappingProfile()
    {
        CreateMap<Vacancy, VacancyShortDto>().ReverseMap();
    }
}