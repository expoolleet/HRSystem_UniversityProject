using Application.Companies.Models.Commands;
using Application.Companies.Models.Response;
using Application.Companies.Models.Response.Responses;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    public AuthorizeUserCommandHandler(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
    }
    
    public async Task<UserResponse> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Login);
        ArgumentException.ThrowIfNullOrEmpty(request.Password);
        // todo: нормальная обработка неправильного юзера
        return await _userRepository.AuthUser(request.Login, request.Password, cancellationToken);
    }
}