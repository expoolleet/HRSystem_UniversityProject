using Application.Candidates.Models.Commands;
using Application.Candidates.Repository;
using MediatR;

namespace Application.Candidates.Handlers.CommandHandlers;

public class AcceptCandidateCommandHandler : IRequestHandler<AcceptCandidateCommand>
{
    private  readonly ICandidateRepository _candidateRepository;
    
    public AcceptCandidateCommandHandler(ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _candidateRepository = candidateRepository;
    }
    
    public async Task Handle(AcceptCandidateCommand request, CancellationToken cancellationToken)
    {
        await _candidateRepository.Accept(request.Candidate, cancellationToken);
    }
}