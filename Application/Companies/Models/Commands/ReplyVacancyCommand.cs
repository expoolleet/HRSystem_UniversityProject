using MediatR;

namespace Application.Companies.Models.Commands;

public class ReplyVacancyCommand : IRequest
{
    public Guid VacancyId { get; set; }
}