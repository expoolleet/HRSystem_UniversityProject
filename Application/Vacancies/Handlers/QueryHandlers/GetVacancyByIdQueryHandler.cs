using Application.Context;
using Application.Vacancies.Models.Queries;
using Application.Vacancies.Repositories;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Handlers.QueryHandlers;

public class GetVacancyByIdQueryHandler : IRequestHandler<GetVacancyByIdQuery, Vacancy>
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUserContext _userContext;
    
    public GetVacancyByIdQueryHandler(IVacancyRepository vacancyRepository, IUserContext userContext)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        ArgumentNullException.ThrowIfNull(userContext);
        _vacancyRepository = vacancyRepository;
        _userContext = userContext;
    }
    
    public async Task<Vacancy> Handle(GetVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        Vacancy vacancy;
        var userId = await _userContext.GetUserId(cancellationToken);
        if (userId == Guid.Empty)
        {
            vacancy = await _vacancyRepository.GetShort(request.VacancyId, cancellationToken);
        }
        else
        {
            vacancy = await _vacancyRepository.Get(request.VacancyId, cancellationToken);
        }
        if (vacancy == null)
        {
            throw new ApplicationException("Vacancy not found");
        }
        return vacancy;
    }
}