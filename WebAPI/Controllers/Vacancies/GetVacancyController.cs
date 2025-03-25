using Application.Vacancies.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Vacancies;

[ApiController]
[Route("api/[controller]")]
public class GetVacancyController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetVacancyController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet("{vacancyId:guid}")]
    public async Task<IActionResult> GetVacancy([FromRoute] Guid vacancyId)
    {
        var query = new GetVacancyQuery
        {
            VacancyId = vacancyId
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}