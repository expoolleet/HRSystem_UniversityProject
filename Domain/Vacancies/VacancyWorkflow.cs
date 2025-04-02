using Domain.Candidates;

namespace Domain.Vacancies;

public sealed class VacancyWorkflow
{
    public IReadOnlyCollection<VacancyWorkflowStep> Steps { get; private init; }
    public DateTime CreationTime { get; private init; }
    
    private VacancyWorkflow(IReadOnlyCollection<VacancyWorkflowStep> steps)
    {
        ArgumentNullException.ThrowIfNull(steps);
        Steps = steps;
        CreationTime = DateTime.UtcNow;
    }
    
    private VacancyWorkflow() { }

    public static VacancyWorkflow Create(IReadOnlyCollection<VacancyWorkflowStep> steps)
        => new(steps);

    public CandidateWorkflow ToCandidate()
        => CandidateWorkflow.Create(Steps.Select(step => step.ToCandidate()).ToArray());
}
