using Application.Companies.Models.Commands;
using Application.Companies.RepositoryInterfaces;
using Application.Companies.Services;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

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