using Application.Companies.Models.Commands;
using Application.Companies.Models.Response.Responses;
using Application.Companies.Models.Services;
using Application.Companies.Repositories;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public AuthorizeUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(tokenService);
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    public async Task<UserResponse> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Login == null)
        {
            throw new ArgumentNullException(nameof(request.Login), "No login provided");
        }

        if (request.Password == null)
        {
            throw new ArgumentNullException(nameof(request.Password), "No password provided");
        }
        
        var user = await _userRepository.Get(request.Login, cancellationToken);

        if (user == null)
        {
            throw new OperationCanceledException("Invalid login");
        }

        if (!user.Password.Verify(request.Password))
        { 
            throw new OperationCanceledException("Invalid password");
        }
        
        var token = _tokenService.GenerateToken(user);

        return UserResponse.Create(user.Id, "User", user.RoleId, token);
    }
}