using Application.Candidates.Repositories;
using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repositories;
using Domain.Candidates;
using MediatR;

namespace Application.Vacancies.Handlers.CommandHandlers;

public class ReplyVacancyCommandHandler : IRequestHandler<ReplyVacancyCommand>
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly ICandidateRepository _candidateRepository;
    
    public ReplyVacancyCommandHandler(IVacancyRepository vacancyRepository, ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _candidateRepository = candidateRepository;
        _vacancyRepository = vacancyRepository;
    }
    
    public async Task Handle(ReplyVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await _vacancyRepository.Get(request.VacancyId, cancellationToken);

        if (vacancy == null)
        {
            throw new ArgumentException("Vacancy not found");
        }
        
        var candidate = vacancy.CreateCandidate(request.Document, request.ReferalId);

        await _candidateRepository.Create(candidate, cancellationToken);
    }
}