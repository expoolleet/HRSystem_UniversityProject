using MediatR;

namespace Application.Vacancies.Models.Commands;

public class ReplyVacancyCommand : IRequest
{
    public Guid VacancyId { get; set; }
}