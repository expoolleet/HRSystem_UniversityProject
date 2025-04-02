using Domain.Candidates;

namespace WebApi.Contracts.Dto.Candidates;

public class CandidateWorkflowStepDto
{
        public Guid? UserId { get; init; }
        public Guid? RoleId { get; init; }
        public int Number { get; init; }
        public CandidateStatus Status { get; init; }
        public string? Feedback { get; init; }
        public DateTime? FeedbackDate { get; init; }
}