    using Application.Companies.Models.Commands;
    using Application.Companies.Models.Responses;
    using Application.Companies.Models.Services;
    using Application.Companies.Repositories;
    using MediatR;

    namespace Application.Companies.Handlers.CommandHandlers;

    public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        public AuthorizeUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, ITokenService tokenService)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(tokenService);
            ArgumentNullException.ThrowIfNull(roleRepository);
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
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
            var role = await _roleRepository.Get(user.RoleId, cancellationToken);
            
            var token = _tokenService.GenerateToken(user, role);
            
            return UserResponse.Create(user.Id, role, token);
        }
    }