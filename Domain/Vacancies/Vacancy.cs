using Domain.Candidates;

namespace Domain.Vacancies;

public sealed class Vacancy
{
    public Guid Id { get; private init; }
    public Guid CompanyId { get; private init; }
    public string? Description { get; private set; }
    public VacancyWorkflow? Workflow { get; private init; }
    
    private Vacancy(Guid id, Guid companyId, string? description, VacancyWorkflow? workflow)
    {
        Id = id;
        CompanyId = companyId;
        Description = description;
        Workflow = workflow;
    }
    
    private Vacancy() { }

    public static Vacancy Create(Guid companyId, string? description, VacancyWorkflow? workflow)
        => new(Guid.NewGuid(), companyId, description, workflow);

    public static Vacancy CreateShort(Guid companyId, string? description)
        => new(Guid.NewGuid(), companyId, description, workflow: null);

    public void Edit(
        string? description)
    {
        ArgumentException.ThrowIfNullOrEmpty(description);
        
        Description = description;
    }
    
    public Candidate CreateCandidate(CandidateDocument candidateDocument, Guid? referralId)
        => Candidate.Create(Id, referralId, candidateDocument, Workflow.ToCandidate());
}
