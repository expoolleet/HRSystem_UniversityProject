using Domain.Candidates;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class ReplyVacancyCommand : IRequest
{
    public Guid VacancyId { get; init; }
    public Guid? ReferalId { get; init; }
    public required CandidateDocument Document { get; init; }
}