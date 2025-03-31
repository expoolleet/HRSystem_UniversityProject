using Domain.Candidates;

namespace WebApi.Contracts.Responses.Candidates;

public class GetCandidatesByFilterResponse
{
    public required IReadOnlyCollection<Candidate> Candidates { get; init; }
}