using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;
using Domain.Vacancies;

namespace DomainTest.Builders;

public class VacancyBuilder : ISpecimenBuilder
{
    private User _user;
    private int _workflowStepsCount;
    public VacancyBuilder(User user, int workflowStepsCount)
    {
        ArgumentNullException.ThrowIfNull(user);
        _user = user;
        _workflowStepsCount = workflowStepsCount;
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(Vacancy).Equals(request))
        {
            return new NoSpecimen();
        }
        
        var steps = new List<VacancyWorkflowStep>();
        
        for (int i = 0; i < _workflowStepsCount; i++)
        {
            var step = VacancyWorkflowStep.Create(
                _user.Id,
                _user.RoleId,
                context.Create<string>(),
                context.Create<int>());
            steps.Add(step);
        }
        
        var workflow = VacancyWorkflow.Create(steps);

        return Vacancy.Create(
            context.Create<Guid>(),
            context.Create<string>(),
            workflow
        );
    }
}