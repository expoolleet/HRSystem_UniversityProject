using Application.Companies.Context;
using Application.Vacancies.Models.Queries;
using Application.Vacancies.Repository;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Handlers.QueryHandlers;

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
        var vacancy = await _vacancyRepository.Get(_userContext.GetUserId(), request.VacancyId, cancellationToken);

        if (vacancy == null)
        {
            throw new ApplicationException("Vacancy not found");
        }
        
        return vacancy;
    }
}