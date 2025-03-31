using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repository;
using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Handlers.CommandHandlers;

public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, Guid>
{
    private readonly IVacancyRepository _vacancyRepository;

    public CreateVacancyCommandHandler(IVacancyRepository vacancyRepository)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        _vacancyRepository = vacancyRepository;
    }
    public async Task<Guid> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = Vacancy.Create(
            request.CompanyId,
            request.Description,
            request.Workflow);
        
        await _vacancyRepository.Create(vacancy, cancellationToken);

        return vacancy.Id;
    }
}