using Application.Vacancies.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Vacancies;

[ApiController]
[Route("api/[controller]")]
public class EditVacancyController : ControllerBase
{
    private readonly IMediator _mediator;

    public EditVacancyController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> EditVacancy([FromBody] EditVacancyCommand? command)
    {
        if (command == null)
        {
            return BadRequest("Request body is required");
        }
        await _mediator.Send(command);
        return Ok(command.Vacancy.Id);
    }
}