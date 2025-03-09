using Application.Companies.Models.Commands;
using Application.Companies.Services;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

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