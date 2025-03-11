using Application.Companies.Models.Commands;
using Application.Companies.RepositoryInterfaces;
using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class EditVacancyCommandHandler : IRequestHandler<EditVacancyCommand>
{
    private readonly IVacancyRepository _vacancyRepository;
    
    public EditVacancyCommandHandler(IVacancyRepository vacancyRepository)
    {
        ArgumentNullException.ThrowIfNull(vacancyRepository);
        _vacancyRepository = vacancyRepository;
    }
    
    public async Task Handle(EditVacancyCommand request, CancellationToken cancellationToken)
    {
        await _vacancyRepository.Edit(request.Vacancy, request.Description, cancellationToken);
    }
}