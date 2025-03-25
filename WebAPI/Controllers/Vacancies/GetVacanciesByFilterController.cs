using Application.Vacancies.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Vacancies;

[ApiController]
[Route("api/[controller]")]
public class GetVacanciesByFilterController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetVacanciesByFilterController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetVacanciesByFilter([FromQuery] GetVacanciesByFilterQuery? query)
    {
        if (query == null)
        {
            return BadRequest("Request body is required");
        }
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}