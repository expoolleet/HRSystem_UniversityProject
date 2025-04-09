using AutoFixture;
using AutoFixture.Kernel;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApiTest.Builders;

public class VacancyWorkflowDtoBuilder : ISpecimenBuilder
{
    private int _stepsCount;
    public VacancyWorkflowDtoBuilder(int stepsCount)
    {
        _stepsCount = stepsCount;
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(VacancyWorkflowDto).Equals(request))
        {
            return new NoSpecimen();
        }

        List<VacancyWorkflowStepDto> steps = new List<VacancyWorkflowStepDto>();
        for (int i = 0; i < _stepsCount; i++)
        {
            VacancyWorkflowStepDto step = new VacancyWorkflowStepDto
            {
                UserId = context.Create<Guid>(),
                RoleId = context.Create<Guid>(),
                Description = context.Create<string>(),
                Number = i + 1
            };
            steps.Add(step);
        }

        return new VacancyWorkflowDto
        {
            Steps = steps
        };
    }
}