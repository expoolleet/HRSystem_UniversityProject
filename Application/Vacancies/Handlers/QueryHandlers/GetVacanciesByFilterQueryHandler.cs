using Application.Contexts;
using Application.Vacancies.Models.Queries;
using Application.Vacancies.Repositories;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Handlers.QueryHandlers;

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
        return await _vacancyRepository.GetCollectionByFilter(
            request.CompanyId, 
            request.Title, 
            cancellationToken);
    }
}