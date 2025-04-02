using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repositories;
using MediatR;

namespace Application.Vacancies.Handlers.CommandHandlers;

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