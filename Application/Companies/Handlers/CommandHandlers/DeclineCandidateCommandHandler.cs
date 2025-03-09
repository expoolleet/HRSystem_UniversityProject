using Application.Companies.Models.Commands;
using Application.Companies.Services;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

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