namespace WebApi.Contracts.Dto.Candidates;

public class CandidateDocumentDto
{
    public required string Name { get; init; }
    public string? Portfolio { get; init; }
    public int WorkExperience { get; init; }
}