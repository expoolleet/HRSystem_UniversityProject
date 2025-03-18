using Domain.Candidates;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class ReplyVacancyCommand : IRequest
{
    public Guid VacancyId { get; set; }
    public Guid? ReferalId { get; set; }
    public required CandidateDocument Document { get; set; }
}