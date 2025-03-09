using Application.Companies.Models.Commands;
using Application.Companies.RepositoryInterfaces;
using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

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