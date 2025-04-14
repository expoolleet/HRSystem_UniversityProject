using AutoFixture;
using AutoFixture.Kernel;
using Domain.Candidates;
using Domain.Companies;

namespace DomainTest.Builders;

public class CandidateBuilder : ISpecimenBuilder
{
    private User _user;
    private int _workflowStepsCount;
    
    public CandidateBuilder(User user, int workflowStepsCount)
    {
        ArgumentNullException.ThrowIfNull(user);
        _user = user;
        _workflowStepsCount = workflowStepsCount;
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(Candidate).Equals(request))
        {
            return new NoSpecimen();
        }

        var steps = new List<CandidateWorkflowStep>();
        
        for (int i = 0; i < _workflowStepsCount; i++)
        {
            var step = CandidateWorkflowStep.Create(
                _user.Id,
                _user.RoleId,
                context.Create<int>());
            steps.Add(step);
        }

        var workflow = CandidateWorkflow.Create(steps);

        var document = CandidateDocument.Create(
            context.Create<string>(),
            context.Create<int>(),
            context.Create<string>(),
                    context.Create<string>());
        
        return Candidate.Create(
            context.Create<Guid>(),
            context.Create<Guid>(),
            document,
            workflow);
    }
}