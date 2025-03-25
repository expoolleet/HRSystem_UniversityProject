using Application.Vacancies.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Vacancies;

[ApiController]
[Route("api/[controller]")]
public class CreateVacancyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreateVacancyController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyCommand? command)
    {
        if (command == null)
        {
            return BadRequest("Request body is required");
        }
        await _mediator.Send(command);
        return Ok(command.Vacancy.Id);
    }
}