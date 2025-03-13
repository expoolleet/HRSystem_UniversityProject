using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repository;
using MediatR;

namespace Application.Vacancies.Handlers.CommandHandlers;

public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand>
{
    private readonly IVacancyRepository _vacancyRepository;

    public CreateVacancyCommandHandler(IVacancyRepository vacancyRepository)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        _vacancyRepository = vacancyRepository;
    }
    public async Task Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        await _vacancyRepository.Create(request.Vacancy, cancellationToken);
    }
}