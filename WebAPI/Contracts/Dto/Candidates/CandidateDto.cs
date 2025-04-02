using Domain.Candidates;

namespace WebApi.Contracts.Dto.Candidates;

public class CandidateDto
{
    public Guid Id { get; init; }
    public Guid VacancyId { get; init; }
    public Guid? ReferralId { get; init; }
    public required CandidateDocumentDto Document { get; init; }
    public required CandidateWorkflowDto Workflow { get; init; }
    public CandidateStatus Status => Workflow.Status;
}