using Application.Companies.Models.Queries;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByNameQueryHandler(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByNameQuery query, CancellationToken cancellationToken)
    {
        return await _userRepository.Get(query.Name, cancellationToken);
    }
}