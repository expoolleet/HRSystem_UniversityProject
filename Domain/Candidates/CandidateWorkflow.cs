using Domain.Companies;

namespace Domain.Candidates;

public sealed class CandidateWorkflow
{
    private CandidateWorkflow(IReadOnlyCollection<CandidateWorkflowStep> steps)
    {
        ArgumentNullException.ThrowIfNull(steps);
        
        Steps = steps;
        CreationTime = DateTime.UtcNow;
    }
    
    private CandidateWorkflow() { }

    public IReadOnlyCollection<CandidateWorkflowStep> Steps { get; private init; }
    public DateTime CreationTime { get; private init; }
    
    public CandidateStatus Status => GetStatus();

    public static CandidateWorkflow Create(IReadOnlyCollection<CandidateWorkflowStep> steps)
        => new(steps);

    public void Approve(User user, string feedback)
    {
        GetCurrentInProcessingStep().Approve(user, feedback);
    }

    public void Reject(User user, string feedback)
    {
        GetCurrentInProcessingStep().Reject(user, feedback);
    }

    public void Restart()
    {
        foreach (var step in Steps)
        {
            step.Restart();
        }
    }

    private CandidateWorkflowStep GetCurrentInProcessingStep()
    {
        var status = GetStatus();

        if (status == CandidateStatus.Approved)
        {
            throw new Exception("Can not change status due: status is permanent (Approved)");
        }

        if (status == CandidateStatus.Rejected)
        {
            throw new Exception("Can not change status due: status is permanent (Rejected)");
        }

        return Steps
            .Where(step => step.Status == CandidateStatus.InProcessing)
            .OrderBy(step => step.Number)
            .First();
    }

    private CandidateStatus GetStatus()
    {
        if (Steps.All(step => step.Status == CandidateStatus.Approved))
        {
            return CandidateStatus.Approved;
        }

        if (Steps.Any(step => step.Status == CandidateStatus.Rejected))
        {
            return CandidateStatus.Rejected;
        }

        return CandidateStatus.InProcessing;
    }
}
