using Application.Companies.Models.Commands;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public AddUserCommandHandler(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
    }
    
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.Add(request.User, cancellationToken);
    }
}