using AutoMapper;
using Domain.Vacancies;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApi.Contracts.MappingProfiles.Vacancies;

public class VacancyWorkflowMappingProfile : Profile
{
    public VacancyWorkflowMappingProfile()
    {
        CreateMap<VacancyWorkflowStepDto, VacancyWorkflowStep>();

        CreateMap<VacancyWorkflowDto, VacancyWorkflow>()
            .ConstructUsing((src, context) => VacancyWorkflow.Create(
                context.Mapper.Map<List<VacancyWorkflowStep>>(src.Steps)
            ));
    }
}