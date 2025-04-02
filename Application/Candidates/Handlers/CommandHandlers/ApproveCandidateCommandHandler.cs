using Application.Candidates.Models.Commands;
using Application.Candidates.Repositories;
using Application.Companies.Repositories;
using MediatR;

namespace Application.Candidates.Handlers.CommandHandlers;

public class ApproveCandidateCommandHandler : IRequestHandler<ApproveCandidateCommand>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IUserRepository _userRepository;

    public ApproveCandidateCommandHandler(ICandidateRepository candidateRepository, IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(candidateRepository);
        ArgumentNullException.ThrowIfNull(userRepository);
        _candidateRepository = candidateRepository;
        _userRepository = userRepository;
    }
    
    public async Task Handle(ApproveCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = _candidateRepository.Get(request.CandidateId, cancellationToken);
        var user = _userRepository.Get(request.UserId, cancellationToken);
        
        await Task.WhenAll(candidate, user);
        
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (candidate == null)
        {
            throw new InvalidOperationException("Candidate not found");
        }
        
        candidate.Result.Approve(user.Result, request.Feedback);
    }
}