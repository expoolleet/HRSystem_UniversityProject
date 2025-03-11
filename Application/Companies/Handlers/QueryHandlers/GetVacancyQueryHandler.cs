using Application.Companies.Context;
using Application.Companies.Models.Queries;
using Application.Companies.RepositoryInterfaces;
using Application.Companies.Services;
using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetVacancyQueryHandler : IRequestHandler<GetVacancyQuery, Vacancy>
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUserContext _userContext;
    
    public GetVacancyQueryHandler(IVacancyRepository vacancyRepository, IUserContext userContext)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        ArgumentNullException.ThrowIfNull(userContext);
        _vacancyRepository = vacancyRepository;
        _userContext = userContext;
    }
    
    public async Task<Vacancy> Handle(GetVacancyQuery request, CancellationToken cancellationToken)
    {
        return await _vacancyRepository.GetVacancy(_userContext.GetUserId(), request.VacancyId, cancellationToken);
    }
}