using Application.Companies.Context;
using Application.Companies.Models.Queries;
using Application.Companies.RepositoryInterfaces;
using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetVacanciesByFilterQueryHandler : IRequestHandler<GetVacanciesByFilterQuery, IReadOnlyCollection<Vacancy>>
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUserContext _userContext;

    public GetVacanciesByFilterQueryHandler(IVacancyRepository vacancyRepository, IUserContext userContext)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        ArgumentNullException.ThrowIfNull(userContext);
        
        _vacancyRepository = vacancyRepository;
        _userContext = userContext;
    }
    
    public async Task<IReadOnlyCollection<Vacancy>> Handle(GetVacanciesByFilterQuery request, CancellationToken cancellationToken)
    {
        return await _vacancyRepository.GetVacanciesByFilter(
            _userContext.GetUserId(), 
            request.CompnayId, 
            request.Title, 
            cancellationToken);
    }
}