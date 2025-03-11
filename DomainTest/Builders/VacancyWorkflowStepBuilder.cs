using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;
using Domain.Vacancies;

namespace DomainTest.Builders;

public class VacancyWorkflowStepBuilder : ISpecimenBuilder
{
    private User _user;
    public VacancyWorkflowStepBuilder(User? user)
    {
        ArgumentNullException.ThrowIfNull(user);
        _user = user;
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(VacancyWorkflowStep).Equals(request))
        {
            return new NoSpecimen();
        }

        return VacancyWorkflowStep.Create(
            _user.Id,
            _user.RoleId,
             context.Create<string>(),
            context.Create<int>());
    }
}