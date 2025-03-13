using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repository;
using MediatR;

namespace Application.Vacancies.Handlers.CommandHandlers;

public class ReplyVacancyCommandHandler : IRequestHandler<ReplyVacancyCommand>
{
    private readonly IVacancyRepository _vacancyRepository;
    public ReplyVacancyCommandHandler(IVacancyRepository vacancyRepository)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        _vacancyRepository = vacancyRepository;
    }
    
    public async Task Handle(ReplyVacancyCommand request, CancellationToken cancellationToken)
    {
        await _vacancyRepository.Reply(request.VacancyId, cancellationToken);
    }
}