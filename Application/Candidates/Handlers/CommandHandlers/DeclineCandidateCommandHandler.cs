using Application.Candidates.Models.Commands;
using Application.Candidates.Repository;
using MediatR;

namespace Application.Candidates.Handlers.CommandHandlers;

public class DeclineCandidateCommandHandler : IRequestHandler<DeclineCandidateCommand>
{
    private  readonly ICandidateRepository _candidateRepository;
    
    public DeclineCandidateCommandHandler(ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _candidateRepository = candidateRepository;
    }
    
    public async Task Handle(DeclineCandidateCommand request, CancellationToken cancellationToken)
    {
        await _candidateRepository.Decline(request.Candidate, cancellationToken);
    }
}