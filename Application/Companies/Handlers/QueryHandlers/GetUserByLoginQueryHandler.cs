using Application.Companies.Models.Queries;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetUserByLoginQueryHandler : IRequestHandler<GetUserByLoginQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByLoginQueryHandler(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByLoginQuery query, CancellationToken cancellationToken)
    {
        return await _userRepository.Get(query.Login, cancellationToken);
    }
}