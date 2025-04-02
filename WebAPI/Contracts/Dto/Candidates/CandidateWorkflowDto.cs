using Domain.Candidates;

namespace WebApi.Contracts.Dto.Candidates;

public class CandidateWorkflowDto
{
    public required IReadOnlyCollection<CandidateWorkflowStepDto> Steps { get; init; }
    public DateTime CreationTime { get; init; }
    public CandidateStatus Status { get; init; }
}