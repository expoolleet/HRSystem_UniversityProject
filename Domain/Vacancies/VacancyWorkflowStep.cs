using Domain.Candidates;

namespace Domain.Vacancies;

public sealed class VacancyWorkflowStep
{
    private VacancyWorkflowStep(Guid? userId, Guid? roleId, string description, int number)
    {
        if (userId is null)
        {
            ArgumentNullException.ThrowIfNull(roleId);
        }

        if (roleId is null)
        {
            ArgumentNullException.ThrowIfNull(userId);
        }

        ArgumentException.ThrowIfNullOrEmpty(description);

        UserId = userId;
        RoleId = roleId;
        Description = description;
        Number = number;
    }

    public Guid? UserId { get; private init; }
    public Guid? RoleId { get; private init; }
    public string Description { get; private init; }
    public int Number { get; private init; }

    public static VacancyWorkflowStep Create(Guid? userId, Guid? roleId, string description, int number)
        => new(userId, roleId, description, number);

    public CandidateWorkflowStep ToCandidate()
        => CandidateWorkflowStep.Create(UserId, RoleId, Number);
}
